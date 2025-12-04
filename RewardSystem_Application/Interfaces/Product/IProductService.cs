using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Product
{
    // Product catalog and metadata operations.
    public interface IProductService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Product.Product> CreateProductAsync(
            string name,
            string? description,
            int requiredPoints,
            int initialStock,
            string? sku,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Product.Product?> GetByIdAsync(Guid productId, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Product.Product>> ListAsync(bool includeInactive = false, CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Product.Product> UpdateProductAsync(
            Guid productId,
            string name,
            string? description,
            int requiredPoints,
            string? sku,
            CancellationToken ct = default);

        Task AdjustStockAsync(Guid productId, int delta, CancellationToken ct = default);

        Task DeactivateAsync(Guid productId, CancellationToken ct = default);
    }
}
