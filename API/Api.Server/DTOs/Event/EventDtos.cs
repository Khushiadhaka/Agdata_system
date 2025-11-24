namespace API.Api.Server.DTOs.Event
{
    public class CreateEventDefinitionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RewardPoints { get; set; }
    }

    public class EventDefinitionResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RewardPoints { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateEventInstanceDto
    {
        public Guid EventDefinitionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class EventInstanceResponseDto
    {
        public Guid Id { get; set; }
        public Guid EventDefinitionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public Guid? WinnerUserId { get; set; }
        public int? Rank { get; set; }
    }
}
