using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.General;

namespace TrustDrop.User.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(UserId), nameof(TenantId), IsUnique = true)]
[Table("user_tenant_role")]
public class UserTenantRoleModel : BaseEntity
{
    public override bool AllowHardDelete => true;
    
    [Required]
    [Column("user_tenant_role_user_id")]
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public UserModel? User { get; set; }

    [Required]
    [Column("user_tenant_role_tenant_id")]
    public Guid TenantId { get; set; }
    [ForeignKey("TenantId")]
    public TenantModel? Tenant { get; set; }

    [Required]
    [Column("user_tenant_role_role")]
    public required UserRole Role { get; set; }
}