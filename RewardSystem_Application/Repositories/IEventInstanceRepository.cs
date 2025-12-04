// Event instance repository abstraction for persistence.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Event instance repository abstraction for persistence.
    public interface IEventInstanceRepository
    {
        Task<EventInstance?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(
            Guid eventDefinitionId,
            CancellationToken ct = default);

        Task AddAsync(EventInstance instance, CancellationToken ct = default);

        Task UpdateAsync(EventInstance instance, CancellationToken ct = default);
    }
}
