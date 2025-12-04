namespace RewardSystem_API.DTOs.Event
{
    // Represents a reward rule associated with an event definition.
    public sealed class EventRewardRuleDto
    {
        // Unique identifier of the reward rule.
        public Guid Id { get; set; }

        // Event definition id this rule belongs to.
        public Guid EventDefinitionId { get; set; }

        // Human readable condition description.
        public string Condition { get; set; } = string.Empty;

        // Points awarded when condition is met.
        public int Points { get; set; }

        // Whether this rule is active.
        public bool IsActive { get; set; }

        // Creation timestamp.
        public DateTime CreatedAt { get; set; }
    }
}
