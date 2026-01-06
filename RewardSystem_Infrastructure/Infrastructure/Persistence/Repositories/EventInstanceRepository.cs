using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for EventInstance entity.
    public sealed class EventInstanceRepository : IEventInstanceRepository
    {
        private readonly RewardDbContext _dbContext;

        public EventInstanceRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<EventInstance> Instances => _dbContext.EventInstances;

        // Get instance by Id.
        public async Task<EventInstance?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Instances.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        // Get all instances.
        public async Task<IReadOnlyList<EventInstance>> GetAllAsync(CancellationToken ct = default)
        {
            return await Instances
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Get instances by EventDefinitionId.
        public async Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(Guid eventDefinitionId, CancellationToken ct = default)
        {
            return await Instances
                .Where(e => e.EventDefinitionId == eventDefinitionId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add instance.
        public async Task AddAsync(EventInstance entity, CancellationToken ct = default)
        {
            await Instances.AddAsync(entity, ct);
        }

        // Update instance.
        public Task UpdateAsync(EventInstance entity, CancellationToken ct = default)
        {
            Instances.Update(entity);
            return Task.CompletedTask;
        }

        // Remove instance.
        public Task RemoveAsync(EventInstance entity, CancellationToken ct = default)
        {
            Instances.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

