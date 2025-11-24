using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Represents a generic business transaction
    public sealed class Transaction : BaseEntity
    {
        // Identifier of the user
        public Guid UserId { get; private set; }

        // Optional identifier of the product
        public Guid? ProductId { get; private set; }

        // Monetary amount of the transaction
        public decimal Amount { get; private set; }

        // Reward points earned in the transaction
        public int RewardPointsEarned { get; private set; }

        // Type of the transaction
        public TransactionType Type { get; private set; }

        // Current status of the transaction
        public TransactionStatus Status { get; private set; }

        // Date and time of the transaction
        public DateTime Date { get; private set; }

        // Parameterless constructor for EF
        private Transaction() { }

        // Creates a new transaction
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

        // Marks transaction as completed
        public void MarkCompleted()
        {
            if (Status == TransactionStatus.Completed)
                throw new BusinessRuleException("Transaction is already completed.");

            Status = TransactionStatus.Completed;
            MarkUpdated();
        }

        // Marks transaction as failed
        public void MarkFailed()
        {
            if (Status == TransactionStatus.Completed)
                throw new BusinessRuleException("Completed transaction cannot be marked as failed.");

            Status = TransactionStatus.Failed;
            MarkUpdated();
        }

        // Updates transaction details
        public void Update(decimal amount, int rewardPointsEarned, TransactionType type)
        {
            if (amount <= 0)
                throw new ValidationException("Amount must be positive.");

            if (rewardPointsEarned < 0)
                throw new ValidationException("Reward points cannot be negative.");

            Amount = amount;
            RewardPointsEarned = rewardPointsEarned;
            Type = type;

            MarkUpdated();
        }
    }

}
