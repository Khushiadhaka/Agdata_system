// Product inventory repository abstraction for persistence.
using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Application.Repositories
{
    // Product inventory repository abstraction for persistence.
    public interface IProductInventoryRepository
    {
        // Get inventory record by product id.
        Task<ProductInventory?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);

        // Add new inventory record.
        Task AddAsync(ProductInventory inventory, CancellationToken ct = default);

        // Update existing inventory record.
        Task UpdateAsync(ProductInventory inventory, CancellationToken ct = default);
    }
}
