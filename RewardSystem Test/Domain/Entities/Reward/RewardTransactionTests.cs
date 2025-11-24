using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Rewards
{
    public sealed class RewardTransactionTests
    {
        [Fact]
        public void Ctor_Should_Create_Transaction_With_Valid_Input()
        {
            // Arrange
            var rewardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var points = 100;
            var type = default(TransactionType); // safe, typed default

            // Act
            var tx = new RewardTransaction(
                rewardId,
                userId,
                points,
                type,
                "ABC-REF"      // reference optional
            );

            // Assert
            tx.RewardId.Should().Be(rewardId);
            tx.UserId.Should().Be(userId);
            tx.PointsGranted.Should().Be(points);
            tx.TransactionType.Should().Be(type);
            tx.Reference.Should().Be("ABC-REF");
        }

        [Fact]
        public void Ctor_Should_Throw_When_RewardId_Empty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var type = default(TransactionType);

            // Act
            Action act = () => new RewardTransaction(
                Guid.Empty,      // ❌ invalid RewardId
                userId,          // ✅ valid UserId
                50,
                type
            );

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Ctor_Should_Throw_When_UserId_Empty()
        {
            // Arrange
            var rewardId = Guid.NewGuid();
            var type = default(TransactionType);

            // Act
            Action act = () => new RewardTransaction(
                rewardId,        // ✅ valid RewardId
                Guid.Empty,      // ❌ invalid UserId
                50,
                type
            );

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Points_Not_Positive()
        {
            // Arrange
            var rewardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var type = default(TransactionType);

            // Act
            Action act = () => new RewardTransaction(
                rewardId,
                userId,
                0,               // ❌ invalid points
                type
            );

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void UpdateReference_Should_Trim_And_Set_Reference()
        {
            // Arrange
            var type = default(TransactionType);

            var tx = new RewardTransaction(
                Guid.NewGuid(),
                Guid.NewGuid(),
                10,
                type,
                "old-ref"
            );

            // Act
            tx.UpdateReference("  new-ref  ");

            // Assert
            tx.Reference.Should().Be("new-ref");
        }

        [Fact]
        public void UpdateReference_Should_Set_Empty_When_Null()
        {
            // Arrange
            var type = default(TransactionType);

            var tx = new RewardTransaction(
                Guid.NewGuid(),
                Guid.NewGuid(),
                10,
                type,
                "old-ref"
            );

            // Act
            tx.UpdateReference(null);

            // Assert
            tx.Reference.Should().BeEmpty();
        }
    }
}
