using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Services
{
    public class RedemptionRecordTests
    {
        // Constructor

        [Fact]
        public void Constructor_ValidInputs_CreatesRecordCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var before = DateTime.UtcNow;

            // Act
            var record = new RedemptionRecord(userId, productId, "  REF-123  ");
            var after = DateTime.UtcNow;

            // Assert
            Assert.Equal(userId, record.UserId);
            Assert.Equal(productId, record.ProductId);
            Assert.Equal("REF-123", record.Reference);

            // RedeemedAt should be between before/after
            Assert.InRange(record.RedeemedAt, before, after);
        }

        [Fact]
        public void Constructor_EmptyUserId_Throws()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ValidationException>(() =>
                new RedemptionRecord(Guid.Empty, productId));
        }

        [Fact]
        public void Constructor_EmptyProductId_Throws()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ValidationException>(() =>
                new RedemptionRecord(userId, Guid.Empty));
        }

        [Fact]
        public void Constructor_NullReference_SetsEmptyString()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            // Act
            var record = new RedemptionRecord(userId, productId, null);

            // Assert
            Assert.Equal(string.Empty, record.Reference);
        }

        // UpdateReference

        [Fact]
        public void UpdateReference_ValidValue_UpdatesReferenceAndUpdatedAt()
        {
            // Arrange
            var record = new RedemptionRecord(Guid.NewGuid(), Guid.NewGuid(), "OLD");
            var before = DateTime.UtcNow;

            // Act
            record.UpdateReference("  NEW-REF  ");
            var after = DateTime.UtcNow;

            // Assert
            Assert.Equal("NEW-REF", record.Reference);
            Assert.NotNull(record.UpdatedAt);
            Assert.InRange(record.UpdatedAt!.Value, before, after);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateReference_NullOrWhitespace_SetsEmptyString(string? reference)
        {
            // Arrange
            var record = new RedemptionRecord(Guid.NewGuid(), Guid.NewGuid(), "OLD");

            // Act
            record.UpdateReference(reference);

            // Assert
            Assert.Equal(string.Empty, record.Reference);
        }
    }
}
