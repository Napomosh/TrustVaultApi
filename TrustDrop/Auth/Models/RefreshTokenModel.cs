using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(TokenHash), IsUnique = true, Name = INDEX_REFRESH_TOKEN_HASH_UNIQUE)]
[Microsoft.EntityFrameworkCore.Index(nameof(UserId), nameof(TenantId), Name = INDEX_REFRESH_TOKEN_USER_TENANT)]
[Table("refresh_token")]
public class RefreshTokenModel : BaseEntity
{
    [NotMapped]
    public const string INDEX_REFRESH_TOKEN_USER_TENANT = "index_refresh_token_user_tenant";
    [NotMapped]   
    public const string INDEX_REFRESH_TOKEN_HASH_UNIQUE = "index_refresh_token_token_hash_unique";
    
    public override bool AllowHardDelete => true;
    
    [Column("refresh_token_user_id")]
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public UserModel? User { get; set; }
    
    [Column("refresh_token_tenant_id")]
    public Guid? TenantId { get; set; }
    [ForeignKey("TenantId")]   
    public TenantModel? Tenant { get; set; }
    
    [Column("refresh_token_hash")]
    [MaxLength(64)]
    public byte[] TokenHash { get; set; } = [];
    
    [Column("refresh_token_expires_at")]
    public DateTime ExpiresAt { get; set; }
    
    [Column("refresh_token_revoked_at")]
    public DateTime? RevokedAt { get; set; }
    
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    [NotMapped]
    public bool IsRevoked => RevokedAt is not null;
}