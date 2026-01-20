using System;

namespace RewardSystem_API.DTOs.Event
{
	// Represents a scheduled event instance.
	public sealed class EventInstanceDto
	{
		public Guid Id { get; set; }
		public Guid EventDefinitionId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool IsCompleted { get; set; }
		public bool IsCancelled { get; set; }
		public Guid? WinnerUserId { get; set; }
		public int? Rank { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
