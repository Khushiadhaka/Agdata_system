using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Transaction
{
    // Request used when creating a new transaction.
    public sealed class TransactionCreateDto
    {
        public Guid UserId { get; set; }                 // user who performed the transaction
        public Guid? RelatedEntityId { get; set; }       // related entity (reward, order, etc.)
        public decimal Amount { get; set; }              // ₹ / $ amount of the transaction
        public int RewardPoints { get; set; }            // points earned in this tx
        public TransactionType Type { get; set; }        // Earn / Redeem / Adjust

    }
}