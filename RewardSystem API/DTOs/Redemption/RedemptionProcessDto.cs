namespace RewardSystem_API.DTOs.Redemption
{
    // Represents internal process state for a redemption (lifecycle).
    public sealed class RedemptionProcessDto
    {
        // Unique process id.
        public Guid Id { get; set; }

        // Business redemption id (request or external id).
        public Guid RedemptionId { get; set; }

        // Points used in this redemption.
        public int PointsUsed { get; set; }

        // Current process status (Pending/Approved/Rejected/Completed/Cancelled).
        public string Status { get; set; } = string.Empty;

        // When the process was created.
        public DateTime CreatedAt { get; set; }
    }
}
