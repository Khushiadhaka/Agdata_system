using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for Reward entity.
    public sealed class RewardRepository : IRewardRepository
    {
        private readonly RewardDbContext _dbContext;

        public RewardRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<Reward> Rewards => _dbContext.Rewards;

        // Get reward by Id.
        public async Task<Reward?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Rewards.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        // Get all rewards.
        public async Task<IReadOnlyList<Reward>> GetAllAsync(CancellationToken ct = default)
        {
            return await Rewards
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add reward.
        public async Task AddAsync(Reward entity, CancellationToken ct = default)
        {
            await Rewards.AddAsync(entity, ct);
        }

        // Update reward.
        public Task UpdateAsync(Reward entity, CancellationToken ct = default)
        {
            Rewards.Update(entity);
            return Task.CompletedTask;
        }

        // Remove reward.
        public Task RemoveAsync(Reward entity, CancellationToken ct = default)
        {
            Rewards.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

