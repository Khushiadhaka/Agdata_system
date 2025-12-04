namespace RewardSystem_API.DTOs.Event
{
    // Represents a scheduled instance of an event definition.
    public sealed class EventInstanceDto
    {
        // Unique identifier of the event instance.
        public Guid Id { get; set; }

        // Associated event definition id.
        public Guid EventDefinitionId { get; set; }

        // Start time of the event instance.
        public DateTime StartTime { get; set; }

        // End time of the event instance.
        public DateTime EndTime { get; set; }

        // Flag indicating if instance is completed.
        public bool IsCompleted { get; set; }

        // Flag indicating if instance is cancelled.
        public bool IsCancelled { get; set; }

        // Winner user id if applicable.
        public Guid? WinnerUserId { get; set; }

        // Rank of the winner (1,2,3...).
        public int? Rank { get; set; }

        // Creation timestamp.
        public DateTime CreatedAt { get; set; }
    }
}
