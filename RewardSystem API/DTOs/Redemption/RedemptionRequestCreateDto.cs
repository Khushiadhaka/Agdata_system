using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Redemption
{
    // Payload to create a new redemption request.
    public sealed class RedemptionRequestCreateDto
    {
        // User requesting redemption.
        public Guid UserId { get; set; }

        // Product the user wants to redeem.
        public Guid ProductId { get; set; }

        // Points the user wants to spend.
        public int PointsUsed { get; set; }
    }

}
