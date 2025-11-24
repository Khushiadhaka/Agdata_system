using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Represents a scheduled instance of an event definition
    public sealed class EventInstance : BaseEntity
    {
        // Identifier of the event definition
        public Guid EventDefinitionId { get; private set; }

        // Start time of the instance
        public DateTime StartTime { get; private set; }

        // End time of the instance
        public DateTime EndTime { get; private set; }

        // Flag indicating completion
        public bool IsCompleted { get; private set; }

        // Flag indicating cancellation
        public bool IsCancelled { get; private set; }

        // Winner user ID (if any)
        public Guid? WinnerUserId { get; private set; }

        // Rank of the winner (1st, 2nd, 3rd...)
        public int? Rank { get; private set; }

        // Parameterless constructor for EF
        private EventInstance() { }

        // Creates a new event instance
        public EventInstance(Guid eventDefinitionId, DateTime startTime, DateTime endTime)
        {
            if (eventDefinitionId == Guid.Empty)
                throw new ValidationException("EventDefinitionId cannot be empty.");

            if (endTime <= startTime)
                throw new ValidationException("End time must be after start time.");

            EventDefinitionId = eventDefinitionId;
            StartTime = startTime;
            EndTime = endTime;
            IsCompleted = false;
            IsCancelled = false;
        }

        // Assign winner
        public void AssignWinner(Guid winnerUserId, int rank)
        {
            if (winnerUserId == Guid.Empty)
                throw new ValidationException("Winner user Id cannot be empty.");

            if (rank <= 0)
                throw new ValidationException("Rank must be greater than zero.");

            WinnerUserId = winnerUserId;
            Rank = rank;
            MarkUpdated();
        }

        // Marks instance as completed
        public void MarkCompleted()
        {
            if (IsCancelled)
                throw new BusinessRuleException("Cancelled instance cannot be completed.");

            if (IsCompleted)
                throw new BusinessRuleException("Instance is already completed.");

            IsCompleted = true;
            MarkUpdated();
        }

        // Cancels the instance
        public void Cancel()
        {
            if (IsCompleted)
                throw new BusinessRuleException("Completed instance cannot be cancelled.");

            if (IsCancelled)
                throw new BusinessRuleException("Instance is already cancelled.");

            IsCancelled = true;
            MarkUpdated();
        }

        // Extends the end time
        public void ExtendEndTime(DateTime newEndTime)
        {
            if (IsCancelled)
                throw new BusinessRuleException("Cancelled instance cannot be modified.");

            if (IsCompleted)
                throw new BusinessRuleException("Completed instance cannot be modified.");

            if (newEndTime <= EndTime)
                throw new ValidationException("New end time must be later than current end time.");

            EndTime = newEndTime;
            MarkUpdated();
        }
    }
}
