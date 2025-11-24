using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents the application of a reward to a user
    public sealed class RewardTransaction : BaseEntity
    {
        // Identifier of the reward
        public Guid RewardId { get; private set; }

        // Identifier of the user receiving the reward
        public Guid UserId { get; private set; }

        // Points granted as part of this reward
        public int PointsGranted { get; private set; }

        // External reference (e.g., transaction id)
        public string Reference { get; private set; } = string.Empty;

        // Type of transaction (Credit/Debit style via TransactionType)
        public TransactionType TransactionType { get; private set; }

        // Optional EventInstance id link
        public Guid? EventInstanceId { get; private set; }

        // Optional RedemptionRequest id link
        public Guid? RedemptionRequestId { get; private set; }

        // Parameterless constructor for EF
        private RewardTransaction() { }

        // Creates a new reward transaction
        public RewardTransaction(
            Guid rewardId,
            Guid userId,
            int pointsGranted,
            TransactionType transactionType,
            string? reference = null,
            Guid? eventInstanceId = null,
            Guid? redemptionRequestId = null)
        {
            if (rewardId == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (pointsGranted <= 0)
                throw new ValidationException("PointsGranted must be greater than zero.");

            RewardId = rewardId;
            UserId = userId;
            PointsGranted = pointsGranted;
            TransactionType = transactionType;
            Reference = reference?.Trim() ?? string.Empty;
            EventInstanceId = eventInstanceId;
            RedemptionRequestId = redemptionRequestId;
        }

        // Updates the reference
        public void UpdateReference(string? reference)
        {
            Reference = reference?.Trim() ?? string.Empty;
            MarkUpdated();
        }
    }
}
