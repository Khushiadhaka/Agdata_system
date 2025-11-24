using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents a points movement (earn or redeem) for audit history
    public sealed class PointsTransaction : BaseEntity
    {
        // ID of the user receiving or spending points
        public Guid UserId { get; private set; }

        // Points earned or redeemed (always positive)
        public int Points { get; private set; }

        // Type of points transaction (Earn / Redeem)
        public PointsTransactionType Type { get; private set; }

        // Optional description
        public string Description { get; private set; } = string.Empty;

        // Parameterless constructor for EF / serializers
        private PointsTransaction() { }

        // Create a new points transaction with validation
        public PointsTransaction(Guid userId, int points, PointsTransactionType type, string? description = null)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            UserId = userId;
            Points = points;
            Type = type;
            Description = description?.Trim() ?? string.Empty;
        }

        // Update description and mark as updated
        public void UpdateDescription(string? description)
        {
            Description = description?.Trim() ?? string.Empty;
            MarkUpdated();
        }
    }
}
