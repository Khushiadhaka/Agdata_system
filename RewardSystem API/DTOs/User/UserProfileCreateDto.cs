using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.User
{
    // Represents payload to create or update a user profile.
    public sealed class UserProfileCreateDto
    {
        // User identifier whose profile is being modified.
        public Guid UserId { get; set; }

        // Phone number for the user.
        public string PhoneNumber { get; set; } = string.Empty;

        // Department name of the user.
        public string Department { get; set; } = string.Empty;

        // Office/location of the user.
        public string Location { get; set; } = string.Empty;
    }
}
