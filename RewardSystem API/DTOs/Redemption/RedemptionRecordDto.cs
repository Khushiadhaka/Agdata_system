namespace RewardSystem_API.DTOs.Redemption
{
    // Represents a completed redemption (product delivered).
    public sealed class RedemptionRecordDto
    {
        // Unique record id.
        public Guid Id { get; set; }

        // User who redeemed the product.
        public Guid UserId { get; set; }

        // Redeemed product id.
        public Guid ProductId { get; set; }

        // When the redemption was completed.
        public DateTime RedeemedAt { get; set; }
    }
}
