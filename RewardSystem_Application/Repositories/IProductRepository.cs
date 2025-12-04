// Product repository abstraction for persistence.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Application.Repositories
{
    // Product repository abstraction for persistence.
    public interface IProductRepository
    {
        // Get a product by id.
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // Get all products.
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);

        // Add a new product.
        Task AddAsync(Product product, CancellationToken ct = default);

        // Update an existing product.
        Task UpdateAsync(Product product, CancellationToken ct = default);

        // Check if a product has pending redemption requests.
        Task<bool> HasPendingRedemptionsAsync(Guid productId, CancellationToken ct = default);
    }
}
