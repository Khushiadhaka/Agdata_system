using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Auth
{
    /// <summary>
    /// Request payload used for user login.
    /// </summary>
    public sealed class LoginRequestDto
    {
        /// <summary>
        /// User email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User password in plain text (for login only).
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
