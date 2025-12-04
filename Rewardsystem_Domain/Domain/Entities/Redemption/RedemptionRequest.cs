using System;
using System.Collections.Generic;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Represents a user's request to redeem points for a product.
    public sealed class RedemptionRequest : BaseEntity
    {
        // Identifier of the requesting user.
        public Guid UserId { get; private set; }

        // Identifier of the product to redeem.
        public Guid ProductId { get; private set; }

        // Points requested to be used for redemption.
        public int PointsUsed { get; private set; }

        // Current status of the request.
        public RedemptionStatus Status { get; private set; }

        // Optional admin note or reference.
        public string Note { get; private set; } = string.Empty;

        // Parameterless ctor for EF Core.
        private RedemptionRequest() { }

        // Main constructor with validation.
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

        // Update requested points (allowed only while pending).
        public void UpdatePoints(int points)
        {
            if (points <= 0)
                throw new ValidationException("PointsUsed must be positive");

            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Points can only be updated for pending requests");

            PointsUsed = points;
            MarkUpdated();
        }

        // Approve the redemption (pending -> approved).
        public void Approve()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be approved");

            Status = RedemptionStatus.Approved;
            MarkUpdated();
        }

        // Reject the redemption (pending -> rejected).
        public void Reject(string? reason = null)
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be rejected");

            Status = RedemptionStatus.Rejected;
            Note = (reason ?? string.Empty).Trim();
            MarkUpdated();
        }

        // Mark as completed after delivery (approved -> completed).
        public void MarkCompleted()
        {
            if (Status != RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved redemptions can be completed.");

            Status = RedemptionStatus.Completed;
            MarkUpdated();
        }

        // Cancel the redemption (allowed unless completed).
        public void Cancel(string? reason = null)
        {
            if (Status == RedemptionStatus.Completed)
                throw new BusinessRuleException("Completed redemptions cannot be cancelled.");

            Status = RedemptionStatus.Cancelled;
            Note = (reason ?? string.Empty).Trim();
            MarkUpdated();
        }

        // Return allowed transitions (helpful for UI).
        public IReadOnlyCollection<RedemptionStatus> GetAllowedTransitions()
        {
            return Status switch
            {
                RedemptionStatus.Pending => new[] { RedemptionStatus.Approved, RedemptionStatus.Rejected, RedemptionStatus.Cancelled },
                RedemptionStatus.Approved => new[] { RedemptionStatus.Completed, RedemptionStatus.Cancelled },
                _ => Array.Empty<RedemptionStatus>()
            };
        }
    }
}
