using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Represents an internal event that can award points
    public sealed class Event : BaseEntity
    {
        // Name of the event
        public string Name { get; private set; } = string.Empty;

        // Description of the event
        public string Description { get; private set; } = string.Empty;

        // Scheduled date of the event
        public DateTime ScheduledDate { get; private set; }

        // Indicates whether the event is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private Event() { }

        // Creates a new event with validation
        public Event(string name, string description, DateTime scheduledDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event name cannot be empty.");

            if (scheduledDate.Date < DateTime.UtcNow.Date)
                throw new ValidationException("Scheduled date cannot be in the past.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            ScheduledDate = scheduledDate.Date;
            IsActive = true;
        }

        // Updates event details
        public void Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event name cannot be empty.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive event.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;

            MarkUpdated();
        }

        // Reschedules the event
        public void Reschedule(DateTime newDate)
        {
            if (newDate.Date < DateTime.UtcNow.Date)
                throw new ValidationException("New date cannot be in the past.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot reschedule an inactive event.");

            ScheduledDate = newDate.Date;
            MarkUpdated();
        }

        // Deactivates the event
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Event is already inactive.");

            IsActive = false;
            MarkUpdated();
        }
    }
}
