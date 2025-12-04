namespace RewardSystem_API.DTOs.Event
{
    // Payload used to create a new event instance for a definition.
    public sealed class EventInstanceCreateDto
    {
        // Event definition id from which to create instance.
        public Guid EventDefinitionId { get; set; }

        // Scheduled start time.
        public DateTime StartTime { get; set; }

        // Scheduled end time.
        public DateTime EndTime { get; set; }
    }
}
