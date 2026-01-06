using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.User
{
    // Represents payload to update an existing user.
    public sealed class UserUpdateDto
    {
        // Identifier of the user to update.
        public Guid Id { get; set; }

        // New full name.
        public string Name { get; set; } = string.Empty;

        // New email address.
        public string Email { get; set; } = string.Empty;

        // New role value.
        public UserRole Role { get; set; }
    }
}
