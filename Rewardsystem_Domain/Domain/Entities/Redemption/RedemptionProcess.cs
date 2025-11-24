using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Manages lifecycle of a redemption operation
    public sealed class RedemptionProcess : BaseEntity
    {
        // Business identifier of the redemption
        public Guid RedemptionId { get; private set; }

        // Points used in the redemption
        public int PointsUsed { get; private set; }

        // Current status of the process
        public RedemptionStatus Status { get; private set; }

        // Parameterless constructor for EF
        private RedemptionProcess() { }

        // Creates a new redemption process
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

        // Approves the redemption
        public void Approve()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be approved.");

            Status = RedemptionStatus.Approved;
            MarkUpdated();
        }

        // Rejects the redemption
        public void Reject()
        {
            if (Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be rejected.");

            Status = RedemptionStatus.Rejected;
            MarkUpdated();
        }

        // Marks redemption as completed
        public void MarkCompleted()
        {
            if (Status != RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved redemptions can be completed.");

            Status = RedemptionStatus.Completed;
            MarkUpdated();
        }

        // Cancels the redemption
        public void Cancel()
        {
            if (Status == RedemptionStatus.Completed)
                throw new BusinessRuleException("Completed redemptions cannot be cancelled.");

            Status = RedemptionStatus.Cancelled;
            MarkUpdated();
        }
    }
}
