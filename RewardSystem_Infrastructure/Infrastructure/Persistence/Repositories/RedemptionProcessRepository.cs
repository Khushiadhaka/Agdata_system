using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for RedemptionProcess entity.
    public sealed class RedemptionProcessRepository : IRedemptionProcessRepository
    {
        private readonly RewardDbContext _dbContext;

        public RedemptionProcessRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<RedemptionProcess> Processes => _dbContext.RedemptionProcesses;

        // Get process by Id.
        public async Task<RedemptionProcess?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Processes.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        // Get all processes.
        public async Task<IReadOnlyList<RedemptionProcess>> GetAllAsync(CancellationToken ct = default)
        {
            return await Processes
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add process.
        public async Task AddAsync(RedemptionProcess entity, CancellationToken ct = default)
        {
            await Processes.AddAsync(entity, ct);
        }

        // Update process.
        public Task UpdateAsync(RedemptionProcess entity, CancellationToken ct = default)
        {
            Processes.Update(entity);
            return Task.CompletedTask;
        }

        // Remove process.
        public Task RemoveAsync(RedemptionProcess entity, CancellationToken ct = default)
        {
            Processes.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

