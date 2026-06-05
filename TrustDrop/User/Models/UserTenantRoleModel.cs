using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.General;

namespace TrustDrop.User.Models;

public class UserTenantRoleModel : BaseEntity
{
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;

    public Guid TenantId { get; set; }
    public TenantModel Tenant { get; set; } = null!;

    public required UserRole Role { get; set; }
}