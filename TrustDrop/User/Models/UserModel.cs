using TrustDrop.Common.Database;
using TrustDrop.User.General;

namespace TrustDrop.User.Models;

public class UserModel : BaseEntity
{
    public required string Username { get; set; }   
    public required string Email { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] Salt { get; set; } = [];
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime? LastLogin { get; set; }
}