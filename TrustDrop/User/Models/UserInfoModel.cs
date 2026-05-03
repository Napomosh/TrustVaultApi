using System.Text.Json.Serialization;

namespace TrustDrop.User.Models;

public class UserInfoModel
{
    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("organizations")]
    public List<string> Organizations { get; set; } = [];

    [JsonPropertyName("previous_entries_ip")]
    public List<string> PreviousEntriesIp { get; set; } = [];
}

