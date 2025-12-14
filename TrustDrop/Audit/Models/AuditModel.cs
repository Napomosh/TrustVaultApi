using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Audit.Models;

[Table("audit")]
public class AuditModel : BaseEntity
{
    [Required]
    [Column("audit_tenant_id")]
    public required Guid TenantId { get; set; }
    [ForeignKey("TenantId")]
    public required TenantModel Tenant { get; set; }

    [Required]
    [Column("audit_actor_id")]
    public required Guid ActorId { get; set; }
    [ForeignKey("ActorId")]
    public required UserModel Actor { get; set; }

    [Required]
    [Column("audit_document_id")]
    public required Guid DocumentId { get; set; }
    [ForeignKey("DocumentId")]
    public required DocumentModel Document { get; set; }

    [Required]
    [Column("audit_action")]
    [MaxLength(255)]
    public required string Action { get; set; }

    [Required]
    [Column("audit_ip")]
    [MaxLength(45)]
    public required string IpAddress { get; set; }

    [Required]
    [Column("audit_ua")]
    [MaxLength(1024)]
    public required string UserAgent { get; set; }

    [Column("audit_details")]
    [MaxLength(2048)]
    public string? Details { get; set; }
}