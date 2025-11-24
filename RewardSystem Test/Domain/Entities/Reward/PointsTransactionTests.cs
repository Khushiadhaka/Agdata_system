using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Rewards
{
    public sealed class PointsTransactionTests
    {
        [Fact]
        public void Ctor_Should_Create_Valid_PointsTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var points = 50;
            var type = PointsTransactionType.Earn;
            var desc = "Earned from event";

            // Act
            var tx = new PointsTransaction(userId, points, type, desc);

            // Assert
            tx.UserId.Should().Be(userId);
            tx.Points.Should().Be(points);
            tx.Type.Should().Be(type);
            tx.Description.Should().Be(desc);
        }

        [Fact]
        public void Ctor_Should_Throw_When_UserId_Empty()
        {
            Action act = () =>
                new PointsTransaction(Guid.Empty, 10, PointsTransactionType.Earn);

            act.Should().Throw<ValidationException>()
               .WithMessage("*UserId cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_Points_Not_Positive()
        {
            Action act = () =>
                new PointsTransaction(Guid.NewGuid(), 0, PointsTransactionType.Earn);

            act.Should().Throw<ValidationException>()
               .WithMessage("*Points must be greater than zero*");
        }

        [Fact]
        public void UpdateDescription_Should_Trim_And_Set_Description()
        {
            var tx = new PointsTransaction(
                Guid.NewGuid(),
                10,
                PointsTransactionType.Earn,
                "old");

            tx.UpdateDescription("  new description  ");

            tx.Description.Should().Be("new description");
        }

        [Fact]
        public void UpdateDescription_Should_Allow_Null_And_Set_Empty_String()
        {
            var tx = new PointsTransaction(
                Guid.NewGuid(),
                10,
                PointsTransactionType.Earn,
                "some desc");

            tx.UpdateDescription(null);

            tx.Description.Should().BeEmpty();
        }
    }
}
