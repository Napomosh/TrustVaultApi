using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Models;

public class RefreshTokenModel : BaseEntity
{
    public override bool AllowHardDelete => true;
    
    public Guid UserId { get; set; }
    public UserModel? User { get; set; }
    
    public Guid? TenantId { get; set; }
    public TenantModel? Tenant { get; set; }
    
    public byte[] TokenHash { get; set; } = [];
    
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt is not null;
}