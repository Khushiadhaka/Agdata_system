namespace RewardSystem_API.DTOs.Reward
{
    // Represents full reward details returned to the client.
    public sealed class RewardDto
    {
        // Unique reward identifier.
        public Guid Id { get; set; }

        // Reward title.
        public string Name { get; set; } = string.Empty;

        // Reward description.
        public string? Description { get; set; }

        // Type of reward (Performance, Project, Innovation etc.)
        public string Type { get; set; } = string.Empty;

        // Whether reward is active.
        public bool IsActive { get; set; }
    }
}
