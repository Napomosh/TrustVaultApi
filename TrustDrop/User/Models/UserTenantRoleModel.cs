using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.General;

namespace TrustDrop.User.Models;

public class UserTenantRoleModel : BaseEntity
{
    public override bool AllowHardDelete => true;
    
    public Guid UserId { get; set; }
    public UserModel? User { get; set; }

    public Guid TenantId { get; set; }
    public TenantModel? Tenant { get; set; }

    public required UserRole Role { get; set; }
}