using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for RedemptionRecord entity.
    public sealed class RedemptionRecordRepository : IRedemptionRecordRepository
    {
        private readonly RewardDbContext _dbContext;

        public RedemptionRecordRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<RedemptionRecord> Records => _dbContext.RedemptionRecords;

        // Get record by Id.
        public async Task<RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Records.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        // Get all records.
        public async Task<IReadOnlyList<RedemptionRecord>> GetAllAsync(CancellationToken ct = default)
        {
            return await Records
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add record.
        public async Task AddAsync(RedemptionRecord entity, CancellationToken ct = default)
        {
            await Records.AddAsync(entity, ct);
        }

        // Update record.
        public Task UpdateAsync(RedemptionRecord entity, CancellationToken ct = default)
        {
            Records.Update(entity);
            return Task.CompletedTask;
        }

        // Remove record.
        public Task RemoveAsync(RedemptionRecord entity, CancellationToken ct = default)
        {
            Records.Remove(entity);
            return Task.CompletedTask;
        }

        // Get records by user.
        public async Task<IReadOnlyList<RedemptionRecord>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await Records
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}

