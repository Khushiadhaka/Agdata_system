using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Interfaces
{
    // Application service for products
    public interface IProductService
    {
        Task<Product> CreateProductAsync(
            string name,
            string description,
            int requiredPoints,
            CancellationToken cancellationToken = default);

        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task UpdateProductAsync(
            Guid id,
            string name,
            string description,
            int requiredPoints,
            CancellationToken cancellationToken = default);

        Task DeactivateProductAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ProductInventory> SetInitialInventoryAsync(
            Guid productId,
            int stockQuantity,
            CancellationToken cancellationToken = default);

        Task IncreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken cancellationToken = default);

        Task ReduceStockAsync(
            Guid productId,
            int quantity,
            CancellationToken cancellationToken = default);
    }
}
