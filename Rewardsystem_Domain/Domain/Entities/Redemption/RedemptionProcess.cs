using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Represents lifecycle and status of a redemption operation (separate from request).
    public sealed class RedemptionProcess : BaseEntity
    {
        // Business identifier of the redemption (could match a domain-level id).
        public Guid RedemptionId { get; private set; }

        // Points used in the redemption operation.
        public int PointsUsed { get; private set; }

        // Current status of the process (tracks flow independently).
        public RedemptionStatus Status { get; private set; }

        // Optional note for operator/admin.
        public string Note { get; private set; } = string.Empty;

        // Parameterless ctor for EF Core.
        private RedemptionProcess() { }

        // Main constructor with validation.
        public RedemptionProcess(Guid redemptionId, int pointsUsed)
        {
            if (redemptionId == Guid.Empty)
                throw new ValidationException("Redemption ID cannot be empty.");

            if (pointsUsed <= 0)
                throw new ValidationException("Points used must be greater than zero.");

            RedemptionId = redemptionId;
            PointsUsed = pointsUsed;
            Status = RedemptionStatus.Pending;
        }

        // Approve the process (pending -> approved).
        public void Approve()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be approved.");

            Status = RedemptionStatus.Approved;
            MarkUpdated();
        }

        // Reject the process (pending -> rejected).
        public void Reject(string? reason = null)
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be rejected.");

            Status = RedemptionStatus.Rejected;
            Note = (reason ?? string.Empty).Trim();
            MarkUpdated();
        }

        // Mark process as completed (approved -> completed).
        public void MarkCompleted()
        {
            if (Status != RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved redemptions can be completed.");

            Status = RedemptionStatus.Completed;
            MarkUpdated();
        }

        // Cancel the process (cannot cancel completed).
        public void Cancel(string? reason = null)
        {
            if (Status == RedemptionStatus.Completed)
                throw new BusinessRuleException("Completed redemptions cannot be cancelled.");

            Status = RedemptionStatus.Cancelled;
            Note = (reason ?? string.Empty).Trim();
            MarkUpdated();
        }
    }
}
