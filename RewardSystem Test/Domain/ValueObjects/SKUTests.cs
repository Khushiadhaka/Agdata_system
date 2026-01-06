using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.ValueObjects;
using System;
using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class SkuTests
    {
        [Fact]
        public void Constructor_ValidValue_SetsValueTrimmed()
        {
            // Arrange
            var raw = "  ABC-123  ";

            // Act
            var sku = new SKU(raw);

            // Assert
            Assert.Equal("ABC-123", sku.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhitespace_ThrowsValidationException(string? value)
        {
            // Act + Assert
            Assert.Throws<ValidationException>(() => new SKU(value!));
        }

        [Fact]
        public void Equality_SameValue_AreEqual()
        {
            // Arrange
            var sku1 = new SKU("ITEM-001");
            var sku2 = new SKU("ITEM-001");

            // Act + Assert
            Assert.Equal(sku1, sku2);
            Assert.True(sku1.Equals(sku2));
            Assert.Equal(sku1.GetHashCode(), sku2.GetHashCode());
        }

        [Fact]
        public void Equality_DifferentValue_AreNotEqual()
        {
            // Arrange
            var sku1 = new SKU("ITEM-001");
            var sku2 = new SKU("ITEM-002");

            // Act + Assert
            Assert.NotEqual(sku1, sku2);
            Assert.False(sku1.Equals(sku2));
        }

        [Fact]
        public void ImplicitConversion_ToString_ReturnsUnderlyingValue()
        {
            // Arrange
            var sku = new SKU("SKU-999");

            // Act
            string str = sku;   // uses implicit operator

            // Assert
            Assert.Equal("SKU-999", str);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            // Arrange
            var sku = new SKU("SKU-XYZ");

            // Act
            var result = sku.ToString();

            // Assert
            Assert.Equal("SKU-XYZ", result);
        }
    }
}
