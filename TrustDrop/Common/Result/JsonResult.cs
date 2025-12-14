using System.Text.Json.Serialization;

namespace TrustDrop.Common.Result;

public class JsonResult<T>(Result<T> rawResult)
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; } = rawResult.IsSuccess;
    
    [JsonPropertyName("value")]
    public T? Value { get; } = rawResult.Value;
    
    [JsonPropertyName("code")]
    public ErrorCode Code { get; } = rawResult.Code;
    
    [JsonPropertyName("error")]
    public string Error { get; } = rawResult.Error;
}