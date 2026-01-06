using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.User
{
    // Represents user account containing reward points and status.
    public sealed class UserAccountDto
    {
        // Account identifier.
        public Guid Id { get; set; }

        // User identifier owning this account.
        public Guid UserId { get; set; }

        // Current reward points balance.
        public int Points { get; set; }

        // Current account status.
        public AccountStatus Status { get; set; }
    }
}
