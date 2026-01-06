using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for Product entity (catalog items).
    public sealed class ProductRepository : IProductRepository
    {
        private readonly RewardDbContext _dbContext;

        public ProductRepository(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<Product> Products => _dbContext.Products;

        // Get product by Id.
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        // Get all products.
        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        {
            return await Products
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Add product.
        public async Task AddAsync(Product entity, CancellationToken ct = default)
        {
            await Products.AddAsync(entity, ct);
        }

        // Update product.
        public Task UpdateAsync(Product entity, CancellationToken ct = default)
        {
            Products.Update(entity);
            return Task.CompletedTask;
        }

        // Remove product.
        public Task RemoveAsync(Product entity, CancellationToken ct = default)
        {
            Products.Remove(entity);
            return Task.CompletedTask;
        }

        // Check if product has pending redemptions.
        public async Task<bool> HasPendingRedemptionsAsync(Guid productId, CancellationToken ct = default)
        {
            return await _dbContext.RedemptionRequests.AnyAsync(
                r => r.ProductId == productId &&
                     r.Status == Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Pending,
                ct);
        }

        // Get inventory record for product.
        public async Task<ProductInventory?> GetInventoryByProductIdAsync(Guid productId, CancellationToken ct = default)
        {
            return await _dbContext.ProductInventories
                .FirstOrDefaultAsync(pi => pi.ProductId == productId, ct);
        }
    }
}

