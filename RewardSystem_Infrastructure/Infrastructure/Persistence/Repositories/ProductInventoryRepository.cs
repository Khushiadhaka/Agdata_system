using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for ProductInventory entity (stock).
    public sealed class ProductInventoryRepository : IProductInventoryRepository
    {
        private readonly RewardDbContext _dbContext;

        public ProductInventoryRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<ProductInventory> Inventories => _dbContext.ProductInventories;

        // Get inventory by Id.
        public async Task<ProductInventory?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Inventories.FirstOrDefaultAsync(pi => pi.Id == id, ct);
        }

        // Get all inventories.
        public async Task<IReadOnlyList<ProductInventory>> GetAllAsync(CancellationToken ct = default)
        {
            return await Inventories
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add inventory.
        public async Task AddAsync(ProductInventory entity, CancellationToken ct = default)
        {
            await Inventories.AddAsync(entity, ct);
        }

        // Update inventory.
        public Task UpdateAsync(ProductInventory entity, CancellationToken ct = default)
        {
            Inventories.Update(entity);
            return Task.CompletedTask;
        }

        // Remove inventory.
        public Task RemoveAsync(ProductInventory entity, CancellationToken ct = default)
        {
            Inventories.Remove(entity);
            return Task.CompletedTask;
        }

        // Get inventory by ProductId.
        public async Task<ProductInventory?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
        {
            return await Inventories.FirstOrDefaultAsync(pi => pi.ProductId == productId, ct);
        }
    }
}

