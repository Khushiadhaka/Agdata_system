using System;
using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Event
{
	// Payload used to create a reward rule for an event definition.
	public sealed class EventRewardRuleCreateDto
	{
		[Required]
		public Guid EventDefinitionId { get; set; }

		[Required]
		[StringLength(500, MinimumLength = 3)]
		public string Condition { get; set; } = string.Empty;

		[Range(1, int.MaxValue, ErrorMessage = "Points must be greater than 0.")]
		public int Points { get; set; }
	}
}
