using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Represents an internal event (e.g., competition, hackathon).
    public sealed class Event : BaseEntity
    {
        // Event name (non-nullable, default empty).
        public string Name { get; private set; } = string.Empty;

        // Event description (non-nullable, default empty).
        public string Description { get; private set; } = string.Empty;

        // Scheduled date of the event (UTC date/time).
        public DateTime ScheduledDate { get; private set; }

        // Whether the event is active.
        public bool IsActive { get; private set; } = true;

        // Parameterless constructor for EF Core.
        private Event() { }

        // Main constructor with validation.
        public Event(string name, string? description, DateTime scheduledDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event name cannot be empty.");

            // scheduled date should not be in the past (business decision).
            if (scheduledDate.Kind == DateTimeKind.Unspecified)
                scheduledDate = DateTime.SpecifyKind(scheduledDate, DateTimeKind.Utc);

            if (scheduledDate.Date < DateTime.UtcNow.Date)
                throw new ValidationException("Scheduled date cannot be in the past.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            ScheduledDate = scheduledDate;
            IsActive = true;
        }

        // Update event metadata.
        public void Update(string name, string? description)
        {
            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive event.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event name cannot be empty.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            MarkUpdated();
        }

        // Reschedule the event to a new date (must be in future).
        public void Reschedule(DateTime newDate)
        {
            if (!IsActive)
                throw new BusinessRuleException("Cannot reschedule an inactive event.");

            if (newDate.Kind == DateTimeKind.Unspecified)
                newDate = DateTime.SpecifyKind(newDate, DateTimeKind.Utc);

            if (newDate.Date < DateTime.UtcNow.Date)
                throw new ValidationException("New date cannot be in the past.");

            ScheduledDate = newDate;
            MarkUpdated();
        }

        // Deactivate the event.
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Event is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivate the event.
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Event is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
