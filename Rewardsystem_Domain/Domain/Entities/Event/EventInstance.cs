using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Represents a scheduled instance of an EventDefinition.
    public sealed class EventInstance : BaseEntity
    {
        // Identifier of the EventDefinition this instance belongs to.
        public Guid EventDefinitionId { get; private set; }

        // Start time (UTC) of the event instance.
        public DateTime StartTime { get; private set; }

        // End time (UTC) of the event instance.
        public DateTime EndTime { get; private set; }

        // Flag indicating whether the instance is completed.
        public bool IsCompleted { get; private set; }

        // Flag indicating whether the instance was cancelled.
        public bool IsCancelled { get; private set; }

        // Optional winner user id (if any).
        public Guid? WinnerUserId { get; private set; }

        // Optional rank of the winner (1 => first place).
        public int? Rank { get; private set; }

        // Parameterless constructor for EF Core.
        private EventInstance() { }

        // Main constructor with validation.
        public EventInstance(Guid eventDefinitionId, DateTime startTime, DateTime endTime)
        {
            if (eventDefinitionId == Guid.Empty)
                throw new ValidationException("EventDefinitionId cannot be empty.");

            if (startTime.Kind == DateTimeKind.Unspecified)
                startTime = DateTime.SpecifyKind(startTime, DateTimeKind.Utc);

            if (endTime.Kind == DateTimeKind.Unspecified)
                endTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc);

            if (endTime <= startTime)
                throw new ValidationException("End time must be after start time.");

            EventDefinitionId = eventDefinitionId;
            StartTime = startTime;
            EndTime = endTime;
            IsCompleted = false;
            IsCancelled = false;
        }

        // Assign a winner with rank.
        public void AssignWinner(Guid winnerUserId, int rank)
        {
            if (winnerUserId == Guid.Empty)
                throw new ValidationException("Winner user id cannot be empty.");

            if (rank <= 0)
                throw new ValidationException("Rank must be greater than zero.");

            WinnerUserId = winnerUserId;
            Rank = rank;
            MarkUpdated();
        }

        // Mark instance as completed (cannot be cancelled afterwards).
        public void MarkCompleted()
        {
            if (IsCancelled)
                throw new BusinessRuleException("Cancelled instance cannot be completed.");

            if (IsCompleted)
                throw new BusinessRuleException("Instance is already completed.");

            IsCompleted = true;
            MarkUpdated();
        }

        // Cancel the instance (if not completed).
        public void Cancel()
        {
            if (IsCompleted)
                throw new BusinessRuleException("Completed instance cannot be cancelled.");

            if (IsCancelled)
                throw new BusinessRuleException("Instance is already cancelled.");

            IsCancelled = true;
            MarkUpdated();
        }

        // Extend the end time (business action).
        public void ExtendEndTime(DateTime newEndTime)
        {
            if (newEndTime.Kind == DateTimeKind.Unspecified)
                newEndTime = DateTime.SpecifyKind(newEndTime, DateTimeKind.Utc);

            if (newEndTime <= EndTime)
                throw new ValidationException("New end time must be later than current end time.");

            if (IsCancelled)
                throw new BusinessRuleException("Cancelled instance cannot be modified.");

            if (IsCompleted)
                throw new BusinessRuleException("Completed instance cannot be modified.");

            EndTime = newEndTime;
            MarkUpdated();
        }
    }
}
