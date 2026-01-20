using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
	// Represents a scheduled instance of an EventDefinition.
	public sealed class EventInstance : BaseEntity
	{
		public Guid EventDefinitionId { get; private set; }
		public DateTime StartTime { get; private set; }
		public DateTime EndTime { get; private set; }
		public bool IsCompleted { get; private set; }
		public bool IsCancelled { get; private set; }
		public Guid? WinnerUserId { get; private set; }
		public int? Rank { get; private set; }

		private EventInstance() { }

		public EventInstance(Guid eventDefinitionId, DateTime startTime, DateTime endTime)
		{
			if (eventDefinitionId == Guid.Empty)
				throw new ValidationException("EventDefinitionId cannot be empty.");

			startTime = EnsureUtc(startTime);
			endTime = EnsureUtc(endTime);

			if (endTime <= startTime)
				throw new ValidationException("End time must be after start time.");

			EventDefinitionId = eventDefinitionId;
			StartTime = startTime;
			EndTime = endTime;
			IsCompleted = false;
			IsCancelled = false;
		}

		// Winner can be assigned ONLY after completion
		public void AssignWinner(Guid winnerUserId, int rank)
		{
			if (!IsCompleted)
				throw new BusinessRuleException("Winner can be assigned only after event completion.");

			if (winnerUserId == Guid.Empty)
				throw new ValidationException("Winner user id cannot be empty.");

			if (rank <= 0)
				throw new ValidationException("Rank must be greater than zero.");

			WinnerUserId = winnerUserId;
			Rank = rank;
			MarkUpdated();
		}

		public void MarkCompleted()
		{
			if (IsCancelled)
				throw new BusinessRuleException("Cancelled instance cannot be completed.");

			if (IsCompleted)
				throw new BusinessRuleException("Instance is already completed.");

			IsCompleted = true;
			MarkUpdated();
		}

		public void Cancel()
		{
			if (IsCompleted)
				throw new BusinessRuleException("Completed instance cannot be cancelled.");

			if (IsCancelled)
				throw new BusinessRuleException("Instance is already cancelled.");

			IsCancelled = true;
			MarkUpdated();
		}

		public void ExtendEndTime(DateTime newEndTime)
		{
			newEndTime = EnsureUtc(newEndTime);

			if (newEndTime <= EndTime)
				throw new ValidationException("New end time must be later than current end time.");

			if (IsCancelled || IsCompleted)
				throw new BusinessRuleException("Cannot modify cancelled or completed instance.");

			EndTime = newEndTime;
			MarkUpdated();
		}

		private static DateTime EnsureUtc(DateTime dt)
		{
			return dt.Kind == DateTimeKind.Unspecified
				? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
				: dt.ToUniversalTime();
		}
	}
}
