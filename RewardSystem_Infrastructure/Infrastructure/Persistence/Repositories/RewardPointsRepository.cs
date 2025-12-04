using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for RewardPoints entity.
    public sealed class RewardPointsRepository : IRewardPointsRepository
    {
        private readonly RewardDbContext _dbContext;

        public RewardPointsRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<RewardPoints> Points => _dbContext.RewardPoints;

        // Get by Id.
        public async Task<RewardPoints?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Points.FirstOrDefaultAsync(rp => rp.Id == id, ct);
        }

        // Get all.
        public async Task<IReadOnlyList<RewardPoints>> GetAllAsync(CancellationToken ct = default)
        {
            return await Points
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add RewardPoints.
        public async Task AddAsync(RewardPoints entity, CancellationToken ct = default)
        {
            await Points.AddAsync(entity, ct);
        }

        // Update RewardPoints.
        public Task UpdateAsync(RewardPoints entity, CancellationToken ct = default)
        {
            Points.Update(entity);
            return Task.CompletedTask;
        }

        // Remove RewardPoints.
        public Task RemoveAsync(RewardPoints entity, CancellationToken ct = default)
        {
            Points.Remove(entity);
            return Task.CompletedTask;
        }

        // Get RewardPoints entries for a RewardId.
        public async Task<IReadOnlyList<RewardPoints>> GetByRewardIdAsync(Guid rewardId, CancellationToken ct = default)
        {
            return await Points
                .Where(rp => rp.RewardId == rewardId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

