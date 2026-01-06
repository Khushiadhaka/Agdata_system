using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for RedemptionRequest entity.
    public sealed class RedemptionRequestRepository : IRedemptionRequestRepository
    {
        private readonly RewardDbContext _dbContext;

        public RedemptionRequestRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<RedemptionRequest> Requests => _dbContext.RedemptionRequests;

        // Get by Id.
        public async Task<RedemptionRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Requests.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        // Get all.
        public async Task<IReadOnlyList<RedemptionRequest>> GetAllAsync(CancellationToken ct = default)
        {
            return await Requests
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add request.
        public async Task AddAsync(RedemptionRequest entity, CancellationToken ct = default)
        {
            await Requests.AddAsync(entity, ct);
        }

        // Update request.
        public Task UpdateAsync(RedemptionRequest entity, CancellationToken ct = default)
        {
            Requests.Update(entity);
            return Task.CompletedTask;
        }

        // Remove request.
        public Task RemoveAsync(RedemptionRequest entity, CancellationToken ct = default)
        {
            Requests.Remove(entity);
            return Task.CompletedTask;
        }

        // Get requests by user.
        public async Task<IReadOnlyList<RedemptionRequest>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await Requests
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Get pending request for user + product.
        public async Task<RedemptionRequest?> GetPendingByUserAndProductAsync(Guid userId, Guid productId, CancellationToken ct = default)
        {
            return await Requests.FirstOrDefaultAsync(
                r => r.UserId == userId &&
                     r.ProductId == productId &&
                     r.Status == Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Pending,
                ct);
        }
    }
}

