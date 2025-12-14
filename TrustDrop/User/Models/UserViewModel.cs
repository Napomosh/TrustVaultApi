using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrustDrop.User.Models;

public class UserViewModel
{
    [JsonPropertyName("login")]
    public required string Login { get; set; }
    
    [JsonPropertyName("password")]
    public required string Password { get; set; }
    
    [JsonPropertyName("email")]
    [EmailAddress]
    public string? Email { get; set; }
}