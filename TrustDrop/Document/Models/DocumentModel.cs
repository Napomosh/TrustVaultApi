using TrustDrop.Common.Database;
using TrustDrop.Policy.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Document.Models;

public class DocumentModel : BaseEntity
{
    public required Guid TenantId { get; set; }
    public required TenantModel Tenant { get; set; }    
    public required string Name { get; set; }    
    public required Guid OwnerId { get; set; }
    public required UserModel Owner { get; set; }    
    public required Guid PolicyId { get; set; }
    public required PolicyModel Policy { get; set; }    
    public required ulong Size { get; set; }   
    public required string Hash { get; set; }    
    public required string ContentType { get; set; }    
    public required string Status { get; set; }   
    public required string StorageKey { get; set; }   
    public DateTime? ScannedAt { get; set; }
    public required DateTime ExpiresAt { get; set; } = DateTime.UtcNow;
}