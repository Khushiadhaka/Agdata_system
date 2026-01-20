using System;

namespace RewardSystem_API.DTOs.Event
{
	// Represents an event definition returned to clients.
	public sealed class EventDefinitionDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public int RewardPoints { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
