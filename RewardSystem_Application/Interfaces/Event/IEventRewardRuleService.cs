using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Event
{
    // Manage reward rules for event definitions
    public interface IEventRewardRuleService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Event.EventRewardRule> CreateAsync(
            Guid eventDefinitionId,
            string condition,
            int points,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Event.EventRewardRule> UpdateAsync(
            Guid ruleId,
            string condition,
            int points,
            CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Event.EventRewardRule>> GetByDefinitionAsync(Guid eventDefinitionId, CancellationToken ct = default);

        Task DeactivateAsync(Guid ruleId, CancellationToken ct = default);
    }
}
