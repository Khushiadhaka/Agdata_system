using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base class for value objects
    public abstract class ValueObject
    {
        // Components used for equality comparison
        protected abstract IEnumerable<object?> GetEqualityComponents();

        // Compares this value object to another
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        // Builds hash code from equality components
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(0, (hash, component) => HashCode.Combine(hash, component));
        }
    }

    // Email value object
    public sealed class Email : ValueObject
    {
        // Email string value
        public string Value { get; }

        // Creates email with validation
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Email cannot be empty.");

            Value = value.Trim().ToLowerInvariant();
        }

        // Equality components
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit conversion to string
        public static implicit operator string(Email email) => email.Value;

        // Friendly ToString
        public override string ToString() => Value;
    }

    // EmployeeId value object
    public sealed class EmployeeId : ValueObject
    {
        // EmployeeId string value
        public string Value { get; }

        // Creates employee id with validation
        public EmployeeId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("EmployeeId cannot be empty.");

            Value = value.Trim();
        }

        // Equality components
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(EmployeeId id) => id.Value;

        public override string ToString() => Value;
    }

    // SKU value object
    public sealed class SKU : ValueObject
    {
        // SKU string value
        public string Value { get; }

        // Creates SKU with validation
        public SKU(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("SKU cannot be empty.");

            Value = value.Trim();
        }

        // Equality components
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(SKU sku) => sku.Value;

        public override string ToString() => Value;
    }
}
