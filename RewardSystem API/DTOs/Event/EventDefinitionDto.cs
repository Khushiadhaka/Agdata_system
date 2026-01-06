namespace RewardSystem_API.DTOs.Event
{
    // Represents an event definition (template) returned to clients.
    public sealed class EventDefinitionDto
    {
        // Unique identifier of the event definition.
        public Guid Id { get; set; }

        // Name of the event definition.
        public string Name { get; set; } = string.Empty;

        // Description of the event definition.
        public string? Description { get; set; }

        // Default reward points for this event type.
        public int RewardPoints { get; set; }

        // Whether this definition is active.
        public bool IsActive { get; set; }

        // Creation timestamp.
        public DateTime CreatedAt { get; set; }
    }
}
