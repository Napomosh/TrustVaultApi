using TrustDrop.Auth.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Dal;

public interface IAuthDal
{
    public Task CreateUser(UserModel userModel);
    public Task<int> UpdateUser(UserModel userModel);
    public Task<UserModel?> GetUser(Guid id);
    public Task<UserModel?> GetUser(string login);
    
    public Task<string> CreateRefreshToken(Guid userId, Guid tenantId);
    public Task<RefreshTokenModel?> GetRefreshToken(byte[] tokenHash);
    public Task<RefreshTokenModel?> GetRefreshToken(Guid tokenId);
    public Task RevokeRefreshToken(Guid tokenId);
    public Task RevokeAllRefreshTokens(Guid userId, Guid tenantId);
}