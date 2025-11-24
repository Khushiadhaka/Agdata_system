using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Represents a user's request to redeem points
    public sealed class RedemptionRequest : BaseEntity
    {
        // Identifier of the requesting user
        public Guid UserId { get; private set; }

        // Identifier of the product
        public Guid ProductId { get; private set; }

        // Points requested to be used
        public int PointsUsed { get; private set; }

        // Current status of the request
        public RedemptionStatus Status { get; private set; }

        // Parameterless constructor for EF
        private RedemptionRequest() { }

        // Creates a new redemption request
        public RedemptionRequest(Guid userId, Guid productId, int pointsUsed)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty");

            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty");

            if (pointsUsed <= 0)
                throw new ValidationException("PointsUsed must be positive");

            UserId = userId;
            ProductId = productId;
            PointsUsed = pointsUsed;
            Status = RedemptionStatus.Pending;
        }

        // Updates the points requested, when pending
        public void UpdatePoints(int pointsUsed)
        {
            if (pointsUsed <= 0)
                throw new ValidationException("PointsUsed must be positive");

            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Points can only be updated for pending requests");

            PointsUsed = pointsUsed;
            MarkUpdated();
        }

        // Approves the redemption request
        public void Approve()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be approved");

            Status = RedemptionStatus.Approved;
            MarkUpdated();
        }

        // Rejects the redemption request
        public void Reject()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be rejected");

            Status = RedemptionStatus.Rejected;
            MarkUpdated();
        }

        // Marks as completed (after product delivered)
        public void MarkCompleted()
        {
            if (Status != RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved redemptions can be completed.");

            Status = RedemptionStatus.Completed;
            MarkUpdated();
        }

        // Allowed transitions (for tests / UI)
        public IReadOnlyCollection<RedemptionStatus> GetAllowedTransitions()
        {
            return Status switch
            {
                RedemptionStatus.Pending => new[] { RedemptionStatus.Approved, RedemptionStatus.Rejected },
                RedemptionStatus.Approved => new[] { RedemptionStatus.Completed },
                _ => Array.Empty<RedemptionStatus>()
            };
        }
    }
}
