// Event definition repository abstraction for persistence.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Event definition repository abstraction for persistence.
    public interface IEventDefinitionRepository
    {
        Task<EventDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<EventDefinition>> GetAllAsync(CancellationToken ct = default);

        Task AddAsync(EventDefinition definition, CancellationToken ct = default);

        Task UpdateAsync(EventDefinition definition, CancellationToken ct = default);
    }
}
