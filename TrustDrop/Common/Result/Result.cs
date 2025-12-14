namespace TrustDrop.Common.Result;

public enum ErrorCode
{
    NotFound = 0,
    TransactionFailed = 1,
    ValidationFailed = 2,
    Forbidden = 3,
    Unauthorized = 4,
    Conflict = 5,
    
    NoError = 200,
    Unknown = int.MaxValue,
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public ErrorCode Code { get; }
    public string Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, T? value, ErrorCode? code, string? error)
    {
        IsSuccess = isSuccess;
        Code = code ?? ErrorCode.NoError;
        Error = error ?? string.Empty;
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(true, value, null, null);

    public static Result<T> Failure(ErrorCode code, string error)
        => new(false, default, code, error);
}