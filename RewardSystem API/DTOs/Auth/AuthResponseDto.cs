namespace RewardSystem_API.DTOs.Auth
{
    // Represents response returned after successful authentication.

    /// <summary>
    /// Response returned from /api/auth/login.
    /// </summary>
    public sealed class AuthResponseDto
    {
        /// <summary>JWT bearer token string.</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>UTC expiry time of the token.</summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>Logged-in user's identifier.</summary>
        public Guid UserId { get; set; }

        /// <summary>Display name of the user.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
