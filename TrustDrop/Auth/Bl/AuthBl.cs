using System.Data;
using System.Security.Cryptography;
using TrustDrop.Auth.Dal;
using TrustDrop.Common.Crypto;
using TrustDrop.Common.Database;
using TrustDrop.Common.Error;
using TrustDrop.Common.Jwt;
using TrustDrop.Common.Result;
using TrustDrop.Common.Result.Auth;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Bl;

public class AuthBl(IAuthDal authDal, ILogger<AuthBl> log, ITransactional transactional) : IAuthBl
{
    private readonly IAuthDal _authDal = authDal;
    private readonly ILogger<AuthBl> _log = log;
    private readonly ITransactional _transactional = transactional;

    public async Task<Result<bool>> RegisterUser(string login, string password, string email)
    {
        if (await _authDal.GetUser(login) != null)
        {
            _log.LogWarning(AuthError.USER_NAME_ALREADY_EXIST + " {login}", login);
            return Result<bool>.Failure(ErrorCode.Conflict, AuthError.USER_NAME_ALREADY_EXIST);
        }

        var salt = RandomNumberGenerator.GetBytes(16);
        var hashedPass = PasswordHasher.HashPassword(password, salt);

        var userModel = new UserModel
        {
            Username = login.Trim(),
            PasswordHash = hashedPass,
            Salt = salt,
            Email = email.Trim().ToLower(),
        };

        try
        {
            await _authDal.CreateUser(userModel);
            _log.LogInformation("User created {userModelUsername}", userModel.Username);
            return Result<bool>.Success(true);
        }
        catch (DuplicateNameException ex)
        {
            var errorMessage = ex.Message;
            switch (errorMessage)
            {
                case DbIndexes.INDEX_USER_USERNAME_UNIQUE:
                {
                    _log.LogWarning(AuthError.USER_NAME_ALREADY_EXIST + " {login}", login);
                    return Result<bool>.Failure(ErrorCode.Conflict, AuthError.USER_NAME_ALREADY_EXIST);
                }
                case DbIndexes.INDEX_USER_EMAIL_UNIQUE:
                {
                    _log.LogWarning(AuthError.USER_EMAIL_ALREADY_EXIST + " { mail}", email);
                    return Result<bool>.Failure(ErrorCode.Conflict, AuthError.USER_EMAIL_ALREADY_EXIST);
                }
            }
        }

        _log.LogWarning(AuthError.USER_UNKNOWN_ERROR + " {userName}", userModel.Username);
        return Result<bool>.Failure(ErrorCode.Unknown, AuthError.USER_UNKNOWN_ERROR);
    }

    public async Task<Result<LoginResult>> LoginUser(string login, string password)
    {
        var normalizedLogin = login.Trim();
        var existedUser = await _authDal.GetUser(normalizedLogin);
        if (existedUser == null)
        {
            var fakeSalt = RandomNumberGenerator.GetBytes(16);
            PasswordHasher.VerifyPassword(password, fakeSalt, new byte[32]);
            _log.LogWarning(AuthError.USER_WRONG_CREDENTIALS + " {login}", login);
            return Result<LoginResult>.Failure(ErrorCode.Unauthorized, AuthError.USER_WRONG_CREDENTIALS);
        }

        if (existedUser.DeletedAt != null)
        {
            _log.LogWarning(AuthError.USER_DISABLED + " {login}", login);
            return Result<LoginResult>.Failure(ErrorCode.Unauthorized, AuthError.USER_DISABLED);
        }

        if (!PasswordHasher.VerifyPassword(password, existedUser.Salt, existedUser.PasswordHash))
        {
            _log.LogWarning(AuthError.USER_WRONG_CREDENTIALS + " {login}", login);
            return Result<LoginResult>.Failure(ErrorCode.Unauthorized, AuthError.USER_WRONG_CREDENTIALS);
        }

        existedUser.LastLogin = DateTime.UtcNow;
        await _authDal.UpdateUser(existedUser);

        var jwt = JwtAuth.GenerateJwtToken(existedUser.Id, existedUser.Role, existedUser.Username, JwtAuth.JwtSettings.Expiration);
        var refreshToken = await _authDal.CreateRefreshToken(existedUser.Id, Guid.Empty);

        var loginResult = new LoginResult
        {
            JwtToken = jwt,
            RefreshToken = refreshToken,
            ExpireAt = DateTime.UtcNow.AddSeconds(JwtAuth.JwtSettings.Expiration),
            UserId = existedUser.Id,
            Username = existedUser.Username,
            Role = existedUser.Role
        };

        return Result<LoginResult>.Success(loginResult);
    }

    public async Task<Result<LoginResult>> RefreshLoginAccess(string refreshToken)
    {
        var refreshTokenHash = SHA256.HashData(Convert.FromBase64String(refreshToken));
        var tokenModel = await _authDal.GetRefreshToken(refreshTokenHash);
        
        if (tokenModel is not null && tokenModel.IsRevoked)
        {
            _log.LogWarning("Refresh token reuse detected for UserId {UserId}. Revoking all tokens.", tokenModel.UserId);
            await _authDal.RevokeAllRefreshTokens(tokenModel.UserId, Guid.Empty);
            return Result<LoginResult>.Failure(ErrorCode.NotFound, AuthError.USER_REFRESH_TOKEN_IS_INVALID);
        }

        if (tokenModel is null || tokenModel.IsExpired)
        {
            _log.LogWarning(AuthError.USER_REFRESH_TOKEN_IS_INVALID + $" UserId is {tokenModel?.UserId}");
            return Result<LoginResult>.Failure(ErrorCode.NotFound, AuthError.USER_REFRESH_TOKEN_IS_INVALID);
        }

        if (tokenModel.User is null)
        {
            _log.LogWarning(AuthError.USER_REFRESH_TOKEN_IS_INVALID + " UserId is null");
            return Result<LoginResult>.Failure(ErrorCode.NotFound, AuthError.USER_REFRESH_TOKEN_IS_INVALID);
        }
        var newJwt = JwtAuth.GenerateJwtToken(tokenModel.User.Id, tokenModel.User.Role
                            , tokenModel.User.Username, JwtAuth.JwtSettings.Expiration);

        try
        {
            await using var transaction = await _transactional.BeginTransactionScopeAsync();

            var newRefreshToken = await _authDal.CreateRefreshToken(tokenModel.User.Id, Guid.Empty);
            await _authDal.RevokeRefreshToken(tokenModel.Id);
            await transaction.CommitAsync();

            var loginResult = new LoginResult
            {
                JwtToken = newJwt,
                RefreshToken = newRefreshToken,
                ExpireAt = DateTime.UtcNow.AddSeconds(JwtAuth.JwtSettings.Expiration),
                UserId = tokenModel.User.Id,
                Username = tokenModel.User.Username,
                Role = tokenModel.User.Role
            };

            return Result<LoginResult>.Success(loginResult);
        }
        catch (OperationCanceledException e)
        {
            _log.LogError(DbError.DB_TRANSACTION_FAILED + $" Exception message: {e.Message}");
            return Result<LoginResult>.Failure(ErrorCode.TransactionFailed, DbError.DB_TRANSACTION_FAILED);
        }
        catch (Exception e)
        {
            _log.LogError(DbError.DB_UNKNOWN_ERROR + $" Exception message: {e.Message}");
            return Result<LoginResult>.Failure(ErrorCode.TransactionFailed, DbError.DB_UNKNOWN_ERROR);
        }
    }

    public async Task LogoutUser(Guid userId, string? refreshToken = null)
    {
        if (refreshToken != null)
        {
            var refreshTokenHash = SHA256.HashData(Convert.FromBase64String(refreshToken));
            var tokenModel = await _authDal.GetRefreshToken(refreshTokenHash);
            if (tokenModel is null || tokenModel.IsExpired || tokenModel.IsRevoked)
            {
                _log.LogWarning(AuthError.USER_REFRESH_TOKEN_IS_INVALID + $" UserId is {userId}");
                return;
            }

            await _authDal.RevokeRefreshToken(tokenModel.Id);
        }
        else
        {
            await _authDal.RevokeAllRefreshTokens(userId, Guid.Empty);
        }
    }
}