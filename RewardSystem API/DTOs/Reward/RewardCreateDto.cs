namespace RewardSystem_API.DTOs.Reward
{
    // Represents payload required to create a new reward.
    public sealed class RewardCreateDto
    {
        // Reward title.
        public string Name { get; set; } = string.Empty;

        // Reward description.
        public string? Description { get; set; }

        // Reward type string enum.
        public string Type { get; set; } = string.Empty;

        // Default reward points to assign.
        public int Points { get; set; }
    }
}
