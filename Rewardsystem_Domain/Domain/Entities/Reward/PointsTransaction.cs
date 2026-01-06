using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents a movement of points (earn or redeem) for audit/history.
    public sealed class PointsTransaction : BaseEntity
    {
        // User who earned or redeemed points.
        public Guid UserId { get; private set; }

        // Number of points moved (always positive).
        public int Points { get; private set; }

        // Type of the points transaction (Earn / Redeem).
        public PointsTransactionType Type { get; private set; }

        // Optional free-text description.
        public string Description { get; private set; } = string.Empty;

        // Parameterless constructor for EF Core.
        private PointsTransaction() { }

        // Main constructor with validation.
        public PointsTransaction(Guid userId, int points, PointsTransactionType type, string? description = null)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            UserId = userId;
            Points = points;
            Type = type;
            Description = (description ?? string.Empty).Trim();
        }

        // Update description if needed.
        public void UpdateDescription(string? description)
        {
            Description = (description ?? string.Empty).Trim();
            MarkUpdated();
        }
    }
}
