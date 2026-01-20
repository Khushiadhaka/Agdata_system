using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Interfaces.Event
{
	// Manages scheduled instances of events.
	public interface IEventInstanceService
	{
		Task<EventInstance> CreateAsync(
			Guid eventDefinitionId,
			DateTime startTime,
			DateTime endTime,
			CancellationToken ct = default);

		Task AssignWinnerAsync(
			Guid instanceId,
			Guid winnerUserId,
			int rank,
			CancellationToken ct = default);

		Task MarkCompletedAsync(
			Guid instanceId,
			CancellationToken ct = default);

		Task CancelAsync(
			Guid instanceId,
			CancellationToken ct = default);

		Task<EventInstance?> GetByIdAsync(
			Guid id,
			CancellationToken ct = default);

		Task<IReadOnlyList<EventInstance>> ListByDefinitionAsync(
			Guid eventDefinitionId,
			CancellationToken ct = default);
	}
}
