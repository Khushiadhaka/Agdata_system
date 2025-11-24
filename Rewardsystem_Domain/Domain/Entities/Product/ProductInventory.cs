using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Product
{
    // Tracks stock quantity for a product
    public sealed class ProductInventory : BaseEntity
    {
        // Identifier of the product
        public Guid ProductId { get; private set; }

        // Current stock quantity
        public int StockQuantity { get; private set; }

        // Indicates whether inventory is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private ProductInventory() { }

        // Creates a new product inventory record
        public ProductInventory(Guid productId, int stockQuantity)
        {
            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            if (stockQuantity < 0)
                throw new ValidationException("Stock quantity cannot be negative.");

            ProductId = productId;
            StockQuantity = stockQuantity;
            IsActive = true;
        }

        // Increases stock quantity
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot modify inventory of inactive product.");

            StockQuantity += quantity;
            MarkUpdated();
        }

        // Reduces stock quantity
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot modify inventory of inactive product.");

            if (quantity > StockQuantity)
                throw new BusinessRuleException("Insufficient stock to reduce.");

            StockQuantity -= quantity;
            MarkUpdated();
        }

        // Deactivates the inventory record
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Inventory is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivates the inventory record
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Inventory is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
