using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IProductInventoryRepository.
    public sealed class InMemoryProductInventoryRepository
        : InMemoryRepositoryBase<ProductInventory>, IProductInventoryRepository
    {
        // Get inventory row by product id.
        public Task<ProductInventory?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
        {
            var inv = _store.Values.FirstOrDefault(i => i.ProductId == productId);
            return Task.FromResult(inv);
        }
    }
}

