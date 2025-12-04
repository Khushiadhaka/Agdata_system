using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Simple record linking user and product when redemption is fulfilled.
    public sealed class RedemptionRecord : BaseEntity
    {
        // User who redeemed the product.
        public Guid UserId { get; private set; }

        // Product that was redeemed.
        public Guid ProductId { get; private set; }

        // When the redemption was fulfilled.
        public DateTime RedeemedAt { get; private set; }

        // Optional external reference (shipment id, vendor id, etc).
        public string Reference { get; private set; } = string.Empty;

        // Parameterless ctor for EF Core.
        private RedemptionRecord() { }

        // Main constructor with validation.
        public RedemptionRecord(Guid userId, Guid productId, string? reference = null)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            UserId = userId;
            ProductId = productId;
            RedeemedAt = DateTime.UtcNow;
            Reference = (reference ?? string.Empty).Trim();
        }

        // Update external reference if needed.
        public void UpdateReference(string? reference)
        {
            Reference = (reference ?? string.Empty).Trim();
            MarkUpdated();
        }
    }
}
