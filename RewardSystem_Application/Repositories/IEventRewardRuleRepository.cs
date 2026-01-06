// Event reward rule repository abstraction for persistence.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Event reward rule repository abstraction for persistence.
    public interface IEventRewardRuleRepository
    {
        Task<EventRewardRule?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<EventRewardRule>> GetByEventDefinitionIdAsync(
            Guid eventDefinitionId,
            CancellationToken ct = default);

        Task AddAsync(EventRewardRule rule, CancellationToken ct = default);

        Task UpdateAsync(EventRewardRule rule, CancellationToken ct = default);
    }
}
