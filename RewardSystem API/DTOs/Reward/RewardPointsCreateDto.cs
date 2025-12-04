using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Reward
{
    // Payload to create a reward points configuration version.
    public sealed class RewardPointsCreateDto
    {
        // Reward id to which points belong.
        public Guid RewardId { get; set; }

        // Points assigned.
        public int Points { get; set; }

        
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
