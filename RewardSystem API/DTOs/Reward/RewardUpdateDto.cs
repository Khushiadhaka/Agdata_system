namespace RewardSystem_API.DTOs.Reward
{
    // Payload for updating an existing reward.
    public sealed class RewardUpdateDto
    {
        // Reward id to update.
        public Guid Id { get; set; }

        // Updated reward name.
        public string Name { get; set; } = string.Empty;

        // Updated reward description.
        public string? Description { get; set; }

        // Updated reward type.
        public string Type { get; set; } = string.Empty;
    }
}
