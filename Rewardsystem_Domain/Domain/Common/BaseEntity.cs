using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base class for all domain entities
    public abstract class BaseEntity
    {
        // Unique identifier for the entity
        public Guid Id { get; protected set; }

        // UTC creation timestamp
        public DateTime CreatedAt { get; protected set; }

        // UTC last update timestamp
        public DateTime? UpdatedAt { get; protected set; }

        // Initializes base properties
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        // Marks entity as updated
        protected void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
