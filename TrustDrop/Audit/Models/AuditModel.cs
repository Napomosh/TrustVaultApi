using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Audit.Models;

public class AuditModel : BaseEntity
{
    public required Guid TenantId { get; set; }
    public required TenantModel Tenant { get; set; }

    public required Guid ActorId { get; set; }
    public required UserModel Actor { get; set; }

    public required Guid DocumentId { get; set; }
    public required DocumentModel Document { get; set; }
    
    public required string Action { get; set; }
    public required string IpAddress { get; set; }
    public required string UserAgent { get; set; }
    public string? Details { get; set; }
}