using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Product
{
    // Tracks stock quantity for a product.
    public sealed class ProductInventory : BaseEntity
    {
        // Identifier of the product this inventory belongs to.
        public Guid ProductId { get; private set; }

        // Current stock quantity (non-negative).
        public int StockQuantity { get; private set; }

        // Whether inventory tracking is active.
        public bool IsActive { get; private set; } = true;

        // Navigation: Product reference (may be null until attached).
        public Product? Product { get; private set; }

        // Parameterless constructor for EF Core.
        private ProductInventory() { }

        // Main constructor with validation.
        public ProductInventory(Guid productId, int initialStock)
        {
            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            if (initialStock < 0)
                throw new ValidationException("Initial stock cannot be negative.");

            ProductId = productId;
            StockQuantity = initialStock;
            IsActive = true;
        }

        // Increase stock by positive quantity.
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity to increase must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot modify inventory of an inactive product.");

            StockQuantity += quantity;
            MarkUpdated();
        }

        // Reduce stock by positive quantity.
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity to reduce must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot modify inventory of an inactive product.");

            if (quantity > StockQuantity)
                throw new BusinessRuleException("Insufficient stock to reduce the requested quantity.");

            StockQuantity -= quantity;
            MarkUpdated();
        }

        // Deactivate inventory tracking (prevent further stock changes).
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Inventory is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivate inventory tracking.
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Inventory is already active.");

            IsActive = true;
            MarkUpdated();
        }

        // Attach the product navigation property (for convenience).
        public void AttachProduct(Product product)
        {
            if (product == null)
                throw new ValidationException("Product cannot be null.");

            if (product.Id != ProductId)
                throw new BusinessRuleException("Product does not match Inventory.ProductId.");

            Product = product;
            MarkUpdated();
        }
    }
}
