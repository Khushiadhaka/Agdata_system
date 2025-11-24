using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Product
{
    // Represents a redeemable product in the catalog
    public sealed class Product : BaseEntity
    {
        // Name of the product
        public string Name { get; private set; } = string.Empty;

        // Description of the product
        public string Description { get; private set; } = string.Empty;

        // Points required to redeem one unit
        public int RequiredPoints { get; private set; }

        // Indicates if the product is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private Product() { }

        // Creates a new product
        public Product(string name, string description, int requiredPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Product name cannot be empty.");

            if (requiredPoints <= 0)
                throw new ValidationException("Required points must be greater than zero.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            RequiredPoints = requiredPoints;
            IsActive = true;
        }

        // Updates product details
        public void Update(string name, string description, int requiredPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Product name cannot be empty.");

            if (requiredPoints <= 0)
                throw new ValidationException("Required points must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive product.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            RequiredPoints = requiredPoints;

            MarkUpdated();
        }

        // Deactivates the product
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Product is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivates the product
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Product is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
