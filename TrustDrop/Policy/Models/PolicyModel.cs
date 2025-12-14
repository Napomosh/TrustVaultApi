using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Policy.Models;

[Table("policy")]
public class PolicyModel : BaseEntity
{
    [Required]
    [Column("policy_tenant_id")]
    public required Guid TenantId { get; set; }
    [ForeignKey("TenantId")]
    public required TenantModel Tenant { get; set; }

    [Column("policy_allowed_user_id")]
    public Guid? AllowedUserId { get; set; }
    [ForeignKey("AllowedUserId")]
    public UserModel? AllowedUser { get; set; }

    [Column("policy_allowed_domain")]
    [MaxLength(255)]
    public string? AllowedDomain { get; set; }

    [Column("policy_max_downloads")]
    public int? MaxDownloads { get; set; }

    [Column("policy_valid_until")]
    public DateTime? ValidUntil { get; set; }
}