using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for PointsTransaction entity.
    public sealed class PointsTransactionRepository : IPointsTransactionRepository
    {
        private readonly RewardDbContext _dbContext;

        public PointsTransactionRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<PointsTransaction> Transactions => _dbContext.PointsTransactions;

        // Get by Id.
        public async Task<PointsTransaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Transactions.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // Get all.
        public async Task<IReadOnlyList<PointsTransaction>> GetAllAsync(CancellationToken ct = default)
        {
            return await Transactions
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add points transaction.
        public async Task AddAsync(PointsTransaction entity, CancellationToken ct = default)
        {
            await Transactions.AddAsync(entity, ct);
        }

        // Update points transaction.
        public Task UpdateAsync(PointsTransaction entity, CancellationToken ct = default)
        {
            Transactions.Update(entity);
            return Task.CompletedTask;
        }

        // Remove points transaction.
        public Task RemoveAsync(PointsTransaction entity, CancellationToken ct = default)
        {
            Transactions.Remove(entity);
            return Task.CompletedTask;
        }

        // Get points transactions by user.
        public async Task<IReadOnlyList<PointsTransaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await Transactions
                .Where(t => t.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

