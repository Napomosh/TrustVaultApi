using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Audit.Models;
using TrustDrop.Common.Database;
using TrustDrop.Policy.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Document.Models;

[Table("document")]
public class DocumentModel : BaseEntity
{
    [Required]
    [Column("document_tenant_id")]
    public required Guid TenantId { get; set; }
    [ForeignKey("TenantId")]
    public required TenantModel Tenant { get; set; }
    
    [Required]
    [Column("document_name")]
    [MaxLength(255)]
    public required string Name { get; set; }
    
    [Required]
    [Column("document_owner_id")]
    public required Guid OwnerId { get; set; }
    [ForeignKey("OwnerId")]
    public required UserModel Owner { get; set; }
    
    [Required]
    [Column("document_policy_id")]
    public required Guid PolicyId { get; set; }
    [ForeignKey("PolicyId")]
    public required PolicyModel Policy { get; set; }
    
    [Required]
    [Column("document_size")]
    public required ulong Size { get; set; }
    
    [Required]
    [Column("document_hash")]
    [MaxLength(1024)]
    public required string Hash { get; set; }
    
    [Required]
    [Column("document_content_type")]
    [MaxLength(255)]
    public required string ContentType { get; set; }
    
    [Required]
    [Column("document_status")]
    [MaxLength(255)]
    public required string Status { get; set; }
    
    [Required]
    [Column("document_storage_key")]
    [MaxLength(1024)]
    public required string StorageKey { get; set; }
    
    [Column("document_scanned_at")]
    public DateTime? ScannedAt { get; set; }
    
    [Column("document_expires_at")]
    public required DateTime ExpiresAt { get; set; } = DateTime.UtcNow;
}