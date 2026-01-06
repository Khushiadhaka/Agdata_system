// Manages reward rules attached to event definitions (create/update/deactivate/list).
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Event;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Services
{
    // Manages reward rules attached to event definitions (create/update/deactivate/list).
    public class EventRewardRuleService : IEventRewardRuleService
    {
        private readonly IEventRewardRuleRepository _repo;
        private readonly IUnitOfWork _uow;

        public EventRewardRuleService(IEventRewardRuleRepository repo, IUnitOfWork uow)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a new reward rule for an event definition.
        public async Task<EventRewardRule> CreateAsync(
            Guid eventDefinitionId,
            string condition,
            int points,
            CancellationToken ct = default)
        {
            if (eventDefinitionId == Guid.Empty)
                throw new ValidationException("EventDefinitionId required.");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ValidationException("Condition required.");
            if (points <= 0)
                throw new ValidationException("Points must be positive.");

            var rule = new EventRewardRule(eventDefinitionId, condition.Trim(), points);
            await _repo.AddAsync(rule, ct);
            await _uow.SaveChangesAsync(ct);
            return rule;
        }

        // Update an existing rule.
        public async Task<EventRewardRule> UpdateAsync(
            Guid ruleId,
            string condition,
            int points,
            CancellationToken ct = default)
        {
            var r = await _repo.GetByIdAsync(ruleId, ct)
                    ?? throw new InvalidOperationException("Rule not found.");

            r.Update(condition.Trim(), points);
            await _repo.UpdateAsync(r, ct);
            await _uow.SaveChangesAsync(ct);
            return r;
        }

        // Get rules by event definition id.
        public async Task<IReadOnlyList<EventRewardRule>> GetByDefinitionAsync(
            Guid eventDefinitionId,
            CancellationToken ct = default)
        {
            var list = await _repo.GetByEventDefinitionIdAsync(eventDefinitionId, ct);
            return list.ToList();
        }

        // Deactivate a rule.
        public async Task DeactivateAsync(Guid ruleId, CancellationToken ct = default)
        {
            var r = await _repo.GetByIdAsync(ruleId, ct)
                    ?? throw new InvalidOperationException("Rule not found.");

            r.Deactivate();
            await _repo.UpdateAsync(r, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
