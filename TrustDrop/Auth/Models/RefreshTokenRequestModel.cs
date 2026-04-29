using System.Text.Json.Serialization;

namespace TrustDrop.Auth.Models;

public class RefreshTokenRequestModel
{
    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; }
}