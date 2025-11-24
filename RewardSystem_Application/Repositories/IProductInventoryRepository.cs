using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    // Repository abstraction for ProductInventory
    public interface IProductInventoryRepository : IRepository<ProductInventory>
    {
        // Get inventory record for specific product
        Task<ProductInventory?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);
    }
}
