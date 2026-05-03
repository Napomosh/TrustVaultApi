using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Tenant.Models;

namespace TrustDrop.AccessToken.Models;

public class AccessTokenModel : BaseEntity
{
    public override bool AllowHardDelete => true;    
    public required Guid TenantId { get; set; }
    public required TenantModel Tenant { get; set; }
    public required Guid DocumentId { get; set; }
    public required DocumentModel Document { get; set; }
    public required string TokenHash { get; set; }
    public bool OneTime { get; set; }
    public required int TtlSeconds { get; set; }
    public int UsedCount { get; set; }
    public DateTime? LastUsedAt { get; set; }
}