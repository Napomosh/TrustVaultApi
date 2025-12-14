using TrustDrop.Common.Result;
using TrustDrop.Common.Result.Auth;

namespace TrustDrop.Auth.Bl;

public interface IAuthBl
{
    public Task<Result<bool>> RegisterUser(string login, string password, string email);
    public Task<Result<LoginResult>> LoginUser(string login, string password);
    public Task<Result<LoginResult>> RefreshLoginAccess(string refreshToken);
    public Task LogoutUser(Guid userId, string? refreshToken = null);
}