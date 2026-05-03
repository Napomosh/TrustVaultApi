using TrustDrop.Common.Database;
using TrustDrop.User.Models;

namespace TrustDrop.Tenant.Models;

public class TenantModel : BaseEntity
{
    public required string Name { get; set; }

    public required Guid OwnerId { get; set; }
    public required UserModel Owner { get; set; } 
}