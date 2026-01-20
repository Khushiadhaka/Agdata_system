using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Event
{
	// Payload used to create a new event definition.
	public sealed class EventDefinitionCreateDto
	{
		[Required]
		[StringLength(200, MinimumLength = 3)]
		public string Name { get; set; } = string.Empty;

		[StringLength(1000)]
		public string? Description { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "RewardPoints must be greater than 0.")]
		public int RewardPoints { get; set; }
	}
}
