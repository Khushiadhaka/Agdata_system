using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for Transaction entity.
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly RewardDbContext _dbContext;

        public TransactionRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<Transaction> Transactions => _dbContext.Transactions;

        // Get transaction by Id.
        public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Transactions.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // Get all transactions.
        public async Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken ct = default)
        {
            return await Transactions
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add transaction.
        public async Task AddAsync(Transaction entity, CancellationToken ct = default)
        {
            await Transactions.AddAsync(entity, ct);
        }

        // Update transaction.
        public Task UpdateAsync(Transaction entity, CancellationToken ct = default)
        {
            Transactions.Update(entity);
            return Task.CompletedTask;
        }

        // Remove transaction.
        public Task RemoveAsync(Transaction entity, CancellationToken ct = default)
        {
            Transactions.Remove(entity);
            return Task.CompletedTask;
        }

        // Get transactions by user.
        public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await Transactions
                .Where(t => t.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

