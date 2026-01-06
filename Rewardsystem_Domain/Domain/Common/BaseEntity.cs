using System;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base class for all domain entities (provides ID + timestamps)
    public abstract class BaseEntity
    {
        // Unique identifier of the entity
        public Guid Id { get; protected set; }

        // Creation timestamp (UTC)
        public DateTime CreatedAt { get; protected set; }

        // Last updated timestamp (UTC) - nullable
        public DateTime? UpdatedAt { get; protected set; }

        // Default constructor sets Id and CreatedAt
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        // Updates the UpdatedAt timestamp
        protected void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
