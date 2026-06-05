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
    public DateTimeOffset? CreatedAt
    {
        get;
        set => field ??= value;
    }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}