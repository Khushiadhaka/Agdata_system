namespace RewardSystem_API.DTOs.User
{
    // Represents user profile information returned to client.
    public sealed class UserProfileDto
    {
        // Profile identifier.
        public Guid Id { get; set; }

        // Associated user identifier.
        public Guid UserId { get; set; }

        // Phone number of the user.
        public string PhoneNumber { get; set; } = string.Empty;

        // Department name of the user.
        public string Department { get; set; } = string.Empty;

        // Office/location of the user.
        public string Location { get; set; } = string.Empty;
    }
}
