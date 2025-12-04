using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.ValueObjects
{
    public sealed class SKU : ValueObject
    {
        public string Value { get; private set; } = null!;

        private SKU() { }

        public SKU(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("SKU cannot be empty.");

            Value = value.Trim();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(SKU sku) => sku.Value;

        public override string ToString() => Value;
    }
}
