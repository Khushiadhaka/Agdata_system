using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.ValueObjects
{
    // Represents an Email Address value object
    public sealed class Email : ValueObject
    {
        // Actual email string
        public string Value { get; }

        // Constructor validates and sets value
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Email cannot be empty.");

            Value = value.Trim().ToLowerInvariant();
        }

        // Required for equality comparison
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit conversion to string
        public static implicit operator string(Email email) => email.Value;

        // Readable string output
        public override string ToString() => Value;
    }
}

