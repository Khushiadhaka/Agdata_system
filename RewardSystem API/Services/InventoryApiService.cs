using AutoMapper;
using RewardSystem_API.DTOs.Product;
using RewardSystem_Application.Interfaces.Inventory;

namespace RewardSystem_API.Services
{
    // Public API service used by InventoryController
    public interface IInventoryApiService
    {
        // Get inventory information for a product
        Task<ProductInventoryDto?> GetInventoryAsync(
            Guid productId,
            CancellationToken cancellationToken = default);

        // Change stock (positive = add, negative = reduce)
        Task<bool> UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken cancellationToken = default);
    }

    // For now this is only a thin placeholder – no domain calls yet
    public sealed class InventoryApiService : IInventoryApiService
    {
        //Inject and use application IInventoryService when method signatures are final.

        public Task<ProductInventoryDto?> GetInventoryAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            // Later: call your real inventory/query service here.
            throw new NotImplementedException(
                "GetInventoryAsync is not wired to the application layer yet.");
        }

        public Task<bool> UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken cancellationToken = default)
        {
            // Later: call your real stock-update method here.
            throw new NotImplementedException(
                "UpdateStockAsync is not wired to the application layer yet.");
        }
    }
}
