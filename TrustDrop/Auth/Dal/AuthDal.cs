using System.Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TrustDrop.Auth.Models;
using TrustDrop.Common.Database;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Dal;

public class AuthDal(AppDbContext _dbContext) : IAuthDal
{
    private readonly AppDbContext _dbContext = _dbContext;
    
    public async Task CreateUser(UserModel userModel)
    { 
        try
        {
            await _dbContext.Users.AddAsync(userModel);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresException)
        {
            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                    throw new DuplicateNameException(postgresException.ConstraintName, postgresException);
                case PostgresErrorCodes.QueryCanceled:
                    throw new TimeoutException("Database operation timeout", ex);
            }
        }
    }

    public async Task<int> UpdateUser(UserModel userModel)
    {
        var existingUser = await _dbContext.Users.FindAsync(userModel.Id);
        if (existingUser is null)
            return 0;
        
        existingUser.Username = userModel.Username;
        existingUser.Email = userModel.Email;
        existingUser.Role = userModel.Role;
        existingUser.LastLogin = userModel.LastLogin;
        
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<UserModel?> GetUser(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<UserModel?> GetUser(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == login) ?? null;
    }

    public async Task<string> CreateRefreshToken(Guid userId, Guid tenantId)
    {
        var refreshToken = RandomNumberGenerator.GetBytes(32);

        var model = new RefreshTokenModel
        {
            TokenHash = SHA256.HashData(refreshToken),
            UserId = userId,
            TenantId = null,
            ExpiresAt = DateTime.UtcNow.AddDays(14)
        };

        await _dbContext.RefreshTokens.AddAsync(model);
        await _dbContext.SaveChangesAsync();

        return Convert.ToBase64String(refreshToken);
    }

    public Task<RefreshTokenModel?> GetRefreshToken(byte[] tokenHash)
    {
        return _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.TokenHash == tokenHash);
    }

    public async Task<RefreshTokenModel?> GetRefreshToken(Guid tokenId)
    {
        return await _dbContext.RefreshTokens.FindAsync(tokenId);
    }

    public async Task RevokeRefreshToken(Guid tokenId)
    {
        var token = await GetRefreshToken(tokenId);
        if (token is not null)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }

    // Bulk revocation using ExecuteUpdateAsync.
    // This method performs a direct SQL UPDATE without using the EF change tracker,
    // so SaveChanges() is NOT required afterwards.
    // If combined with other EF operations in the same unit of work,
    // wrap all operations in a transaction to maintain atomicity.
    public async Task RevokeAllRefreshTokens(Guid userId, Guid tenantId)
    {
        var now = DateTime.UtcNow;
        
        await _dbContext.RefreshTokens
            .Where(r => r.UserId == userId && r.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.RevokedAt, now));
    }
}