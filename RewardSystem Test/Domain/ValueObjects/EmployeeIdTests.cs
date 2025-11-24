using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;

using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class EmployeeIdTests
    {
        [Fact]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            var id = new EmployeeId("EMP123");

            id.Value.Should().Be("EMP123");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Empty(string value)
        {
            Action act = () => new EmployeeId(value);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Equality_Should_Use_Value()
        {
            var e1 = new EmployeeId("EMP1");
            var e2 = new EmployeeId("EMP1");

            e1.Equals(e2).Should().BeTrue();
            e1.GetHashCode().Should().Be(e2.GetHashCode());
        }
    }
}
