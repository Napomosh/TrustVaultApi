using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustDrop.Common.Database;

public class BaseEntity
{
    [NotMapped]
    public virtual bool AllowHardDelete => false;
    
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}