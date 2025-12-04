using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for RewardTransaction entity.
    public sealed class RewardTransactionRepository : IRewardTransactionRepository
    {
        private readonly RewardDbContext _dbContext;

        public RewardTransactionRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<RewardTransaction> Transactions => _dbContext.RewardTransactions;

        // Get by Id.
        public async Task<RewardTransaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Transactions.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // Get all.
        public async Task<IReadOnlyList<RewardTransaction>> GetAllAsync(CancellationToken ct = default)
        {
            return await Transactions
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add transaction.
        public async Task AddAsync(RewardTransaction entity, CancellationToken ct = default)
        {
            await Transactions.AddAsync(entity, ct);
        }

        // Update transaction.
        public Task UpdateAsync(RewardTransaction entity, CancellationToken ct = default)
        {
            Transactions.Update(entity);
            return Task.CompletedTask;
        }

        // Remove transaction.
        public Task RemoveAsync(RewardTransaction entity, CancellationToken ct = default)
        {
            Transactions.Remove(entity);
            return Task.CompletedTask;
        }

        // Get transactions by user.
        public async Task<IReadOnlyList<RewardTransaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await Transactions
                .Where(t => t.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

