using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;

using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class SKUTests
    {
        [Fact]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            var sku = new SKU("PROD-1");

            sku.Value.Should().Be("PROD-1");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Empty(string value)
        {
            Action act = () => new SKU(value);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Equality_Should_Use_Value()
        {
            var s1 = new SKU("ABC-123");
            var s2 = new SKU("ABC-123");

            s1.Equals(s2).Should().BeTrue();
            s1.GetHashCode().Should().Be(s2.GetHashCode());
        }
    }
}
