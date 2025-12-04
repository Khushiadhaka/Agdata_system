using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for EventRewardRule entity.
    public sealed class EventRewardRuleRepository : IEventRewardRuleRepository
    {
        private readonly RewardDbContext _dbContext;

        public EventRewardRuleRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<EventRewardRule> Rules => _dbContext.EventRewardRules;

        // Get rule by Id.
        public async Task<EventRewardRule?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Rules.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        // Get all rules.
        public async Task<IReadOnlyList<EventRewardRule>> GetAllAsync(CancellationToken ct = default)
        {
            return await Rules
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Get rules by EventDefinitionId.
        public async Task<IReadOnlyList<EventRewardRule>> GetByEventDefinitionIdAsync(Guid eventDefinitionId, CancellationToken ct = default)
        {
            return await Rules
                .Where(r => r.EventDefinitionId == eventDefinitionId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add rule.
        public async Task AddAsync(EventRewardRule entity, CancellationToken ct = default)
        {
            await Rules.AddAsync(entity, ct);
        }

        // Update rule.
        public Task UpdateAsync(EventRewardRule entity, CancellationToken ct = default)
        {
            Rules.Update(entity);
            return Task.CompletedTask;
        }

        // Remove rule.
        public Task RemoveAsync(EventRewardRule entity, CancellationToken ct = default)
        {
            Rules.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

