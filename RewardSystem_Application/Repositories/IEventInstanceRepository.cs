using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Repository abstraction for EventInstance
    public interface IEventInstanceRepository : IRepository<EventInstance>
    {
        // Get all instances of a given event definition
        Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(
            Guid eventDefinitionId,
            CancellationToken cancellationToken = default);
    }
}
