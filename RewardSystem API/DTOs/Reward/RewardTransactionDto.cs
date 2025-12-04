using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Reward
{
    // Represents reward transaction details.
    public sealed class RewardTransactionDto
    {
        public Guid Id { get; set; }       // Unique transaction id
        public Guid RewardId { get; set; } // Reward performed
        public Guid UserId { get; set; }   // User who received points
        public int PointsGranted { get; set; } // Points credited
        public string TransactionType { get; set; } = string.Empty; // Credit or something else
        public string? Reference { get; set; } // Optional reference text
        public DateTime CreatedAt { get; set; } // Timestamp
    }
}
