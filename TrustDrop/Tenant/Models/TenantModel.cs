using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.AccessToken.Models;
using TrustDrop.Audit.Models;
using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Policy.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Tenant.Models;

[Table("tenant")]
public class TenantModel : BaseEntity
{
    [Required]
    [Column("tenant_name")]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [Column("tenant_owner_id")]
    public required Guid OwnerId { get; set; }
    [ForeignKey("OwnerId")]
    public required UserModel Owner { get; set; } 
}