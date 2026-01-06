using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IProductRepository.
    public sealed class InMemoryProductRepository
        : InMemoryRepositoryBase<Product>, IProductRepository
    {
        // Check if product has pending redemptions (always false in plain in-memory version).
        public Task<bool> HasPendingRedemptionsAsync(Guid productId, CancellationToken ct = default)
        {
            // For in-memory demo, assume no pending redemptions.
            return Task.FromResult(false);
        }

        // Get inventory row for a product (overridden in inventory repo, but kept for interface).
        public Task<ProductInventory?> GetInventoryByProductIdAsync(Guid productId, CancellationToken ct = default)
        {
            // In pure IProductRepository we usually don’t return inventory;
            // real logic is in InMemoryProductInventoryRepository.
            return Task.FromResult<ProductInventory?>(null);
        }

        // Search products by name substring.
        public Task<IReadOnlyList<Product>> SearchByNameAsync(string namePart, CancellationToken ct = default)
        {
            var term = namePart.Trim().ToLowerInvariant();
            var list = _store.Values
                .Where(p => p.Name.ToLower().Contains(term))
                .ToList();

            return Task.FromResult<IReadOnlyList<Product>>(list);
        }
    }
}

