using System;

namespace RewardSystem_API.DTOs.Event
{
	// Represents a reward rule associated with an event definition.
	public sealed class EventRewardRuleDto
	{
		public Guid Id { get; set; }
		public Guid EventDefinitionId { get; set; }
		public string Condition { get; set; } = string.Empty;
		public int Points { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
