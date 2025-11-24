using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Repository for EventDefinition
    public interface IEventDefinitionRepository : IRepository<EventDefinition>
    {
        Task<EventDefinition?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EventDefinition>> GetActiveAsync(
            CancellationToken cancellationToken = default);
    }
}
