using System;
using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Event
{
	// Payload used to create a new event instance.
	public sealed class EventInstanceCreateDto
	{
		[Required]
		public Guid EventDefinitionId { get; set; }

		[Required]
		public DateTime StartTime { get; set; }

		[Required]
		public DateTime EndTime { get; set; }
	}
}
