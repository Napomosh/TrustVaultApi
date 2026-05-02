namespace TrustDrop.Common.Jwt;

public class JwtSettingsType
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    // seconds
    public int Expiration { get; set; } = 120;
}