using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Redemption
{
    // Represents a redemption request made by a user.
    public sealed class RedemptionRequestDto
    {
        // Unique identifier of the redemption request.
        public Guid Id { get; set; }

        // User who requested redemption.
        public Guid UserId { get; set; }

        // Product the user wants to redeem.
        public Guid ProductId { get; set; }

        // Points that will be used for this redemption.
        public int PointsUsed { get; set; }

        // Current status of the request (Pending/Approved/Rejected/Completed).
        public string Status { get; set; } = string.Empty;

        // When the request was created.
        public DateTime CreatedAt { get; set; }
    }
}
