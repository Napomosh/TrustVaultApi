using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.AccessToken.Models;

[Table("access_token")]
public class AccessTokenModel : BaseEntity
{
    public override bool AllowHardDelete => true;
    
    [Required]
    [Column("access_token_tenant_id")]
    public required Guid TenantId { get; set; }
    [ForeignKey("TenantId")]
    public required TenantModel Tenant { get; set; }

    [Required]
    [Column("access_token_document_id")]
    public required Guid DocumentId { get; set; }
    [ForeignKey("DocumentId")]
    public required DocumentModel Document { get; set; }

    [Required]
    [Column("access_token_hash")]
    [MaxLength(1024)]
    public required string TokenHash { get; set; }

    [Required]
    [Column("access_token_one_time")]
    public bool OneTime { get; set; }

    [Required]
    [Column("access_token_ttl_seconds")]
    public required int TtlSeconds { get; set; }

    [Column("access_token_used_count")]
    public int UsedCount { get; set; }

    [Column("access_token_last_used_at")]
    public DateTime? LastUsedAt { get; set; }
}