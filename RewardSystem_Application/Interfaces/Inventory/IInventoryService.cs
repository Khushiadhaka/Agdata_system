using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Application.Interfaces.Inventory
{
    /// <summary>
    /// Inventory related operations for products.
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Returns inventory record for the given product id, or null if not found.
        /// </summary>
        Task<ProductInventory?> GetByProductIdAsync(
            Guid productId,
            CancellationToken ct = default);

        /// <summary>
        /// Increases stock quantity for a product.
        /// </summary>
        Task IncreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Decreases stock quantity for a product.
        /// </summary>
        Task DecreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Changes stock by a delta. Positive = increase, negative = decrease.
        /// (Used by API layer, wraps Increase / Decrease.)
        /// </summary>
        Task UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken ct = default);
    }
}
