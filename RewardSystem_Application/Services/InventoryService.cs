using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Inventory;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Application.Services
{
    /// <summary>
    /// Performs inventory queries and stock increase/decrease operations.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly IProductInventoryRepository _inventoryRepo;
        private readonly IUnitOfWork _uow;

        public InventoryService(
            IProductInventoryRepository inventoryRepo,
            IUnitOfWork uow)
        {
            _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Get inventory record by product id.
        /// </summary>
        public async Task<ProductInventory?> GetByProductIdAsync(
            Guid productId,
            CancellationToken ct = default)
        {
            if (productId == Guid.Empty)
                return null;

            return await _inventoryRepo.GetByProductIdAsync(productId, ct);
        }

        /// <summary>
        /// Increase stock quantity.
        /// </summary>
        public async Task IncreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity must be positive.");

            var inv = await _inventoryRepo.GetByProductIdAsync(productId, ct)
                      ?? throw new InvalidOperationException("Inventory not found.");

            inv.IncreaseStock(quantity);

            await _inventoryRepo.UpdateAsync(inv, ct);
            await _uow.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Decrease stock quantity.
        /// </summary>
        public async Task DecreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity must be positive.");

            var inv = await _inventoryRepo.GetByProductIdAsync(productId, ct)
                      ?? throw new InvalidOperationException("Inventory not found.");

            inv.ReduceStock(quantity);

            await _inventoryRepo.UpdateAsync(inv, ct);
            await _uow.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Change stock by delta. Positive = increase, negative = decrease.
        /// This keeps API layer simple (one method).
        /// </summary>
        public async Task UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken ct = default)
        {
            if (quantityChange > 0)
            {
                await IncreaseStockAsync(productId, quantityChange, ct);
            }
            else if (quantityChange < 0)
            {
                // quantityChange is negative, so pass absolute value
                await DecreaseStockAsync(productId, Math.Abs(quantityChange), ct);
            }
            // if 0, nothing to do
        }
    }
}
