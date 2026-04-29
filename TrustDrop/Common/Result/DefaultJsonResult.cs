using System.Text.Json.Serialization;

namespace TrustDrop.Common.Result;

public class DefaultJsonResult<T>
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; } = true;
    
    [JsonPropertyName("value")]
    public T? Value { get; init; }

    [JsonPropertyName("code")]
    public ErrorCode Code { get; } = ErrorCode.NoError;
    
    [JsonPropertyName("error")]
    public string Error { get; } = string.Empty;
}