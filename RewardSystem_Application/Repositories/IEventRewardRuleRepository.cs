using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Repository abstraction for EventRewardRule
    public interface IEventRewardRuleRepository : IRepository<EventRewardRule>
    {
        // Get all rules for a given event definition
        Task<IReadOnlyList<EventRewardRule>> GetByDefinitionIdAsync(
            Guid eventDefinitionId,
            CancellationToken cancellationToken = default);
    }
}
