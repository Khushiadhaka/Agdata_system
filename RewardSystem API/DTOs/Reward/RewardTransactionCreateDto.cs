using Rewardsystem_Domain.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Reward
{
    /// <summary>
    /// DTO used to create a reward transaction for a user.
    /// </summary>
    public sealed class RewardTransactionCreateDto
    {
        // User who receives the reward points
        public Guid UserId { get; set; }

        // Reward that is being granted
        public Guid RewardId { get; set; }

        // Number of points granted to the user
        public int PointsGranted { get; set; }

        // Type of transaction (earn / adjustment / etc.)
        public TransactionType TransactionType { get; set; }

        // free-text notes for this transaction
        public string? Notes { get; set; }

        // related transaction id (e.g., redemption transaction)
        public Guid? RelatedTransactionId { get; set; }
    }
}
