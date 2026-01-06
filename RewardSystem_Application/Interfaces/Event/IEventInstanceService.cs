using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Event
{
    // Manage scheduled instances of events
    public interface IEventInstanceService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Event.EventInstance> CreateAsync(
            Guid eventDefinitionId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken ct = default);

        Task AssignWinnerAsync(Guid instanceId, Guid winnerUserId, int rank, CancellationToken ct = default);

        Task MarkCompletedAsync(Guid instanceId, CancellationToken ct = default);

        Task CancelAsync(Guid instanceId, CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Event.EventInstance?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Event.EventInstance>> ListByDefinitionAsync(Guid eventDefinitionId, CancellationToken ct = default);
    }
}
