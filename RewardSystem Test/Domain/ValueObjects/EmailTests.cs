using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.ValueObjects;
using Xunit;

namespace RewardSystem_Test.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Constructor_ValidEmail_SetsValueLowercasedAndTrimmed()
        {
            // Arrange
            var raw = "  USER@Example.COM  ";

            // Act
            var email = new Email(raw);

            // Assert
            Assert.Equal("user@example.com", email.Value);
            Assert.Equal("user@example.com", email.ToString());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhitespace_ThrowsValidationException(string? value)
        {
            // Act + Assert
            Assert.Throws<ValidationException>(() => new Email(value!));
        }

        [Fact]
        public void Equality_SameValue_IsEqual()
        {
            // Arrange
            var e1 = new Email("user@example.com");
            var e2 = new Email("USER@EXAMPLE.COM"); 

            // Act + Assert
            Assert.Equal(e1, e2);
            Assert.True(e1.Equals(e2));
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void Equality_DifferentValue_IsNotEqual()
        {
            // Arrange
            var e1 = new Email("user1@example.com");
            var e2 = new Email("user2@example.com");

            // Act + Assert
            Assert.NotEqual(e1, e2);
            Assert.False(e1.Equals(e2));
        }

        [Fact]
        public void ImplicitConversion_ReturnsUnderlyingString()
        {
            // Arrange
            var email = new Email("user@example.com");

            // Act
            string value = email; // implicit operator

            // Assert
            Assert.Equal("user@example.com", value);
        }
    }
}
