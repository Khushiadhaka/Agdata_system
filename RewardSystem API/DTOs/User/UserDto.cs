using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.User
{
    // Represents basic user information.
    public sealed class UserDto
    {
        // Unique identifier of the user.
        public Guid Id { get; set; }

        // Full display name of the user.
        public string Name { get; set; } = string.Empty;

        // Email address of the user.
        public string Email { get; set; } = string.Empty;

        // Employee id text of the user.
        public string EmployeeId { get; set; } = string.Empty;

        // Role of the user (Admin/User).
        public UserRole Role { get; set; }

        // Soft delete flag.
        public bool IsDeleted { get; set; }
    }
}
