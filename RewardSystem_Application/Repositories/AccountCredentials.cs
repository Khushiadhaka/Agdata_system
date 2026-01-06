using System;

namespace RewardSystem_Application.Repositories
{
    // Simple DTO to hold stored credentials for a user.
    public sealed class AccountCredentials
    {
        // Id of the related user.
        public Guid UserId { get; init; }

        // Hashed password for the user.
        public string PasswordHash { get; init; } = string.Empty;

        // Optional last updated time (for auditing).
        public DateTime? UpdatedAt { get; init; }

        // Convenience constructor.
        public AccountCredentials(Guid userId, string passwordHash, DateTime? updatedAt = null)
        {
            UserId = userId;
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            UpdatedAt = updatedAt;
        }

        // Parameterless ctor for serializers / EF etc.
        public AccountCredentials()
        {
        }
    }
}

