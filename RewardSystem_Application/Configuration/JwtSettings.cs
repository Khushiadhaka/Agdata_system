// Strongly-typed settings for JWT configuration.
namespace RewardSystem_Application.Configuration
{
    // JWT settings bound from configuration (e.g. appsettings.json: "JwtSettings").
    public sealed class JwtSettings
    {
        // Token issuer (who created the token).
        public string Issuer { get; set; } = string.Empty;

        // Token audience (who the token is intended for).
        public string Audience { get; set; } = string.Empty;

        // Symmetric secret key used to sign the token.
        public string Secret { get; set; } = string.Empty;

        // Expiration time in minutes for generated tokens.
        public int ExpiryMinutes { get; set; } = 60;
    }
}

