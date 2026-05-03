namespace TrustDrop.Common.Database;

public static class DbIndexes
{
    public const string INDEX_USER_USERNAME_UNIQUE = "index_user_username_unique";
    public const string INDEX_USER_EMAIL_UNIQUE = "index_user_email_unique";
    public const string INDEX_REFRESH_TOKEN_TOKEN_HASH_UNIQUE = "index_refresh_token_token_hash_unique";
    public const string INDEX_REFRESH_TOKEN_USER_TENANT = "index_refresh_token_user_tenant";
}
