using System.Collections.Generic;
using System.Linq;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base class for all value objects
    public abstract class ValueObject
    {
        // Components used for equality comparison
        protected abstract IEnumerable<object?> GetEqualityComponents();

        // Override for value equality
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            return GetEqualityComponents()
                .SequenceEqual(((ValueObject)obj).GetEqualityComponents());
        }

        // Generate hash code from equality components
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(0, (hash, comp) => HashCode.Combine(hash, comp));
        }
    }
}
