using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.User
{
    // Represents payload to create/register a new user.
    public sealed class UserCreateDto
    {
        // Full name for the new user.
        public string Name { get; set; } = string.Empty;

        // Email address for the new user.
        public string Email { get; set; } = string.Empty;

        // Employee id code for the new user.
        public string EmployeeId { get; set; } = string.Empty;

        // Password for the new user.
        public string Password { get; set; } = string.Empty;

        // Desired role for the new user.
        public UserRole Role { get; set; } = UserRole.User;
    }
}
