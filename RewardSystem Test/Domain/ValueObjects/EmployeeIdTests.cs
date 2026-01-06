using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.ValueObjects;
using System;
using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class EmployeeIdTests
    {
        [Fact]
        public void Constructor_ValidValue_SetsValueTrimmed()
        {
            // Arrange
            var raw = "  EMP-123  ";

            // Act
            var id = new EmployeeId(raw);

            // Assert
            Assert.Equal("EMP-123", id.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhitespace_ThrowsValidationException(string? value)
        {
            // Act + Assert
            Assert.Throws<ValidationException>(() => new EmployeeId(value!));
        }

        [Fact]
        public void Equality_SameValue_AreEqual()
        {
            // Arrange
            var id1 = new EmployeeId("EMP-001");
            var id2 = new EmployeeId("EMP-001");

            // Act + Assert
            Assert.Equal(id1, id2);
            Assert.True(id1.Equals(id2));
            Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
        }

        [Fact]
        public void Equality_DifferentValue_AreNotEqual()
        {
            // Arrange
            var id1 = new EmployeeId("EMP-001");
            var id2 = new EmployeeId("EMP-002");

            // Act + Assert
            Assert.NotEqual(id1, id2);
            Assert.False(id1.Equals(id2));
        }

        [Fact]
        public void ImplicitConversion_ReturnsValueString()
        {
            // Arrange
            var id = new EmployeeId("EMP-999");

            // Act
            string str = id; // implicit operator

            // Assert
            Assert.Equal("EMP-999", str);
        }

        [Fact]
        public void ToString_ReturnsUnderlyingValue()
        {
            // Arrange
            var id = new EmployeeId("EMP-XYZ");

            // Act
            var result = id.ToString();

            // Assert
            Assert.Equal("EMP-XYZ", result);
        }
    }
}
