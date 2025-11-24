using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            var email = new Email("user@agdata.com");

            email.Value.Should().Be("user@agdata.com");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Empty(string value)
        {
            Action act = () => new Email(value);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Equality_Should_Be_Based_On_Value()
        {
            var e1 = new Email("user@agdata.com");
            var e2 = new Email("user@agdata.com");

            e1.Equals(e2).Should().BeTrue();
            e1.GetHashCode().Should().Be(e2.GetHashCode());
        }
    }
}
