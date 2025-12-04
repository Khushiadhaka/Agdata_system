namespace RewardSystem_API.DTOs.Event
{
    // Payload used to create a new reward rule for an event definition.
    public sealed class EventRewardRuleCreateDto
    {
        // Event definition id this rule is attached to.
        public Guid EventDefinitionId { get; set; }

        // Rule condition text.
        public string Condition { get; set; } = string.Empty;

        // Points to award for this rule.
        public int Points { get; set; }
    }
}
