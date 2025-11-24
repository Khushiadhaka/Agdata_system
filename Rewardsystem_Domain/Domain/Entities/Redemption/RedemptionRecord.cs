using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Redemption
{
    // Simple record linking user and product for redemption
    public sealed class RedemptionRecord : BaseEntity
    {
        // Identifier of the user
        public Guid UserId { get; private set; }

        // Identifier of the product
        public Guid ProductId { get; private set; }

        // When redeemed
        public DateTime RedeemedAt { get; private set; }

        // Parameterless constructor for EF
        private RedemptionRecord() { }

        // Creates a new redemption record
        public RedemptionRecord(Guid userId, Guid productId)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("User ID cannot be empty.");

            if (productId == Guid.Empty)
                throw new ValidationException("Product ID cannot be empty.");

            UserId = userId;
            ProductId = productId;
            RedeemedAt = DateTime.UtcNow;
        }
    }
}
