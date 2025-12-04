namespace RewardSystem_API.DTOs.Reward
{
    // Represents historical reward points configuration.
    public sealed class RewardPointsDto
    {
        // Unique record id.
        public Guid Id { get; set; }

        // Reward id.
        public Guid RewardId { get; set; }

        // Points value.
        public int Points { get; set; }

        // Date when this configuration becomes active.
        public DateTime? EffectiveFrom { get; set; }

        // Date when configuration expires.
        public DateTime? EffectiveTo { get; set; }
    }
}
