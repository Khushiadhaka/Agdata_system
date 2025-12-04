using System.ComponentModel.DataAnnotations;
using System;

namespace RewardSystem_API.DTOs.Event
{

    // Payload used to create a new event definition.
    public sealed class EventDefinitionCreateDto
    {
        // Name of the new event definition.
        public string Name { get; set; } = string.Empty;

        // Description of the new event definition.
        public string? Description { get; set; }

        // Default reward points to grant for this event.
        public int RewardPoints { get; set; }
    }
}
