using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.ValueObjects
{
    // Represents an Employee ID value object
    public sealed class EmployeeId : ValueObject
    {
        // Underlying string value
        public string Value { get; }

        // Constructor with validation
        public EmployeeId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("EmployeeId cannot be empty.");

            Value = value.Trim();
        }

        // Components for equality
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit conversion
        public static implicit operator string(EmployeeId id) => id.Value;

        public override string ToString() => Value;
    }
}

