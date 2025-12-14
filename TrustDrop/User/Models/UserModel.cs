using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustDrop.Audit.Models;
using TrustDrop.Common.Database;
using TrustDrop.Document.Models;
using TrustDrop.User.General;

namespace TrustDrop.User.Models;

[Table("user")]
[Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true, Name = INDEX_USER_EMAIL_UNIQUE)]
[Microsoft.EntityFrameworkCore.Index(nameof(Username), IsUnique = true, Name = INDEX_USER_USERNAME_UNIQUE)]
public class UserModel : BaseEntity
{
    [NotMapped]
    public const string INDEX_USER_EMAIL_UNIQUE = "index_user_email_unique";
    [NotMapped]
    public const string INDEX_USER_USERNAME_UNIQUE = "index_user_username_unique";
    
    [Required]
    [Column("user_username")]
    [MaxLength(255)]
    public required string Username { get; set; }
    
    [Required]
    [Column("user_email")]
    [EmailAddress]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Column("user_password_hash", TypeName = "bytea")]
    [MaxLength(32)]
    public byte[] PasswordHash { get; set; } = [];
    
    [Column("user_salt", TypeName = "bytea")]
    [MaxLength(16)]
    public byte[] Salt { get; set; } = [];
    
    [Column("user_role")]
    public UserRole Role { get; set; } = UserRole.User;
    
    [Column("user_last_login")]
    public DateTime? LastLogin { get; set; }
}