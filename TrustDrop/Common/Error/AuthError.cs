namespace TrustDrop.Common.Error;

public static class AuthError
{
    public const string USER_ALREADY_EXIST= "User already exists";
    public const string USER_NAME_ALREADY_EXIST = "User with this login already exists";
    public const string USER_EMAIL_ALREADY_EXIST = "User with this email already exists";
    public const string USER_UNKNOWN_ERROR = "Unknown error while creating user";
    public const string USER_WRONG_CREDENTIALS = "Invalid login or password";
    public const string USER_DISABLED = "User is disabled";
    public const string USER_REFRESH_TOKEN_IS_INVALID = "User refresh token is invalid";
}