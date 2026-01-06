using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Represents a business transaction that may produce reward points.
    public sealed class Transaction : BaseEntity
    {
        // Identifier of the user who performed the transaction.
        public Guid UserId { get; private set; }

        // Optional product associated with the transaction.
        public Guid? ProductId { get; private set; }

        // Monetary amount of the transaction.
        public decimal Amount { get; private set; }

        // Reward points earned as part of this transaction.
        public int RewardPointsEarned { get; private set; }

        // Type of the transaction (purchase, refund, etc).
        public TransactionType Type { get; private set; }

        // Current status of the transaction (Pending, Completed, Failed).
        public TransactionStatus Status { get; private set; }

        // Date/time when the transaction occurred.
        public DateTime Date { get; private set; }

        // Parameterless constructor for EF Core.
        private Transaction() { }

        // Main constructor with validation.
        public Transaction(Guid userId, Guid? productId, decimal amount, int rewardPointsEarned, TransactionType type)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (amount <= 0)
                throw new ValidationException("Amount must be positive.");

            if (rewardPointsEarned < 0)
                throw new ValidationException("Reward points cannot be negative.");

            UserId = userId;
            ProductId = productId;
            Amount = amount;
            RewardPointsEarned = rewardPointsEarned;
            Type = type;
            Status = TransactionStatus.Pending;
            Date = DateTime.UtcNow;
        }

        // Mark the transaction as completed (idempotent guard included).
        public void MarkCompleted()
        {
            if (Status == TransactionStatus.Completed)
                throw new BusinessRuleException("Transaction is already completed.");

            Status = TransactionStatus.Completed;
            MarkUpdated();
        }

        // Mark the transaction as failed.
        public void MarkFailed()
        {
            if (Status == TransactionStatus.Completed)
                throw new BusinessRuleException("Completed transaction cannot be marked as failed.");

            Status = TransactionStatus.Failed;
            MarkUpdated();
        }

        // Update transaction details before completion (amount and points).
        public void Update(decimal amount, int rewardPointsEarned, TransactionType type)
        {
            if (amount <= 0)
                throw new ValidationException("Amount must be positive.");

            if (rewardPointsEarned < 0)
                throw new ValidationException("Reward points cannot be negative.");

            if (Status == TransactionStatus.Completed)
                throw new BusinessRuleException("Completed transaction cannot be modified.");

            Amount = amount;
            RewardPointsEarned = rewardPointsEarned;
            Type = type;
            MarkUpdated();
        }
    }
}
