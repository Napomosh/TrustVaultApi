using TrustDrop.Common.Database;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Policy.Models;

public class PolicyModel : BaseEntity
{
    public required Guid TenantId { get; set; }
    public required TenantModel Tenant { get; set; }

    public Guid? AllowedUserId { get; set; }
    public UserModel? AllowedUser { get; set; }

    public string? AllowedDomain { get; set; }

    public int? MaxDownloads { get; set; }

    public DateTime? ValidUntil { get; set; }
}