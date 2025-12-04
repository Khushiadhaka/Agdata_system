using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for EventDefinition entity.
    public sealed class EventDefinitionRepository : IEventDefinitionRepository
    {
        private readonly RewardDbContext _dbContext;

        public EventDefinitionRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<EventDefinition> Definitions => _dbContext.EventDefinitions;

        // Get definition by Id.
        public async Task<EventDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Definitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        // Get all definitions.
        public async Task<IReadOnlyList<EventDefinition>> GetAllAsync(CancellationToken ct = default)
        {
            return await Definitions
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add definition.
        public async Task AddAsync(EventDefinition entity, CancellationToken ct = default)
        {
            await Definitions.AddAsync(entity, ct);
        }

        // Update definition.
        public Task UpdateAsync(EventDefinition entity, CancellationToken ct = default)
        {
            Definitions.Update(entity);
            return Task.CompletedTask;
        }

        // Remove definition.
        public Task RemoveAsync(EventDefinition entity, CancellationToken ct = default)
        {
            Definitions.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

