using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents application of a reward to a user (audit + linking to event/redemption).
    public sealed class RewardTransaction : BaseEntity
    {
        // Identifier of the reward (source).
        public Guid RewardId { get; private set; }

        // User receiving the reward.
        public Guid UserId { get; private set; }

        // Points granted as part of this reward.
        public int PointsGranted { get; private set; }

        // Optional external reference (e.g., vendor transaction id).
        public string Reference { get; private set; } = string.Empty;

        // Transaction type (credit/debit style, reuse enum if appropriate).
        public TransactionType TransactionType { get; private set; }

        // Optional link to an EventInstance (if reward came from event).
        public Guid? EventInstanceId { get; private set; }

        // Optional link to a RedemptionRequest (if reward is related to redemption).
        public Guid? RedemptionRequestId { get; private set; }

        // Parameterless constructor for EF Core.
        private RewardTransaction() { }

        // Main constructor with validation.
        public RewardTransaction(Guid rewardId, Guid userId, int pointsGranted, TransactionType transactionType, string? reference = null, Guid? eventInstanceId = null, Guid? redemptionRequestId = null)
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
            Reference = (reference ?? string.Empty).Trim();
            EventInstanceId = eventInstanceId;
            RedemptionRequestId = redemptionRequestId;
        }

        // Update the external reference if needed.
        public void UpdateReference(string? reference)
        {
            Reference = (reference ?? string.Empty).Trim();
            MarkUpdated();
        }
    }
}
