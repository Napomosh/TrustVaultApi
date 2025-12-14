using TrustDrop.User.General;

namespace TrustDrop.Common.Result.Auth;

public class LoginResult
{
    public string JwtToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public UserRole Role { get; init; } 
    // public Guid TenantId { get; init; }
}