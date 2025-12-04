using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Event
{
    // Payload used to update an existing event definition.
    public sealed class EventDefinitionUpdateDto
    {
        // Identifier of the event definition to update.
        public Guid Id { get; set; }

        // Updated name.
        public string Name { get; set; } = string.Empty;

        // Updated description.
        public string? Description { get; set; }

        // Updated default reward points.
        public int RewardPoints { get; set; }
    }
}
