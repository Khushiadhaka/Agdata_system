using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Xunit;

namespace RewardSystem_Test.Domain.Rewards
{
    public sealed class RewardPointsTests
    {
        [Fact]
        public void Ctor_Should_Create_Valid_RewardPoints()
        {
            // Arrange
            var rewardId = Guid.NewGuid();
            var points = 100;
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 12, 31);

            // Act
            var rp = new RewardPoints(rewardId, points, from, to);

            // Assert
            rp.RewardId.Should().Be(rewardId);
            rp.Points.Should().Be(points);
            rp.EffectiveFrom.Should().Be(from);
            rp.EffectiveTo.Should().Be(to);
        }

        [Fact]
        public void Ctor_Should_Throw_When_RewardId_Empty()
        {
            Action act = () =>
                new RewardPoints(Guid.Empty, 100, null, null);

            act.Should().Throw<ValidationException>()
               .WithMessage("*RewardId cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_Points_Not_Positive()
        {
            Action act = () =>
                new RewardPoints(Guid.NewGuid(), 0, null, null);

            act.Should().Throw<ValidationException>()
               .WithMessage("*Points must be greater than zero*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_EffectiveTo_Before_Or_Equal_EffectiveFrom()
        {
            var rewardId = Guid.NewGuid();
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 1);

            Action act = () =>
                new RewardPoints(rewardId, 100, from, to);

            act.Should().Throw<ValidationException>()
               .WithMessage("*EffectiveTo must be after EffectiveFrom*");
        }

        [Fact]
        public void UpdatePoints_Should_Change_Values_When_Valid()
        {
            var rewardId = Guid.NewGuid();
            var rp = new RewardPoints(rewardId, 100, null, null);

            var newFrom = new DateTime(2025, 2, 1);
            var newTo = new DateTime(2025, 12, 31);

            rp.UpdatePoints(200, newFrom, newTo);

            rp.Points.Should().Be(200);
            rp.EffectiveFrom.Should().Be(newFrom);
            rp.EffectiveTo.Should().Be(newTo);
        }

        [Fact]
        public void UpdatePoints_Should_Throw_When_Points_Not_Positive()
        {
            var rewardId = Guid.NewGuid();
            var rp = new RewardPoints(rewardId, 100, null, null);

            Action act = () => rp.UpdatePoints(0, null, null);

            act.Should().Throw<ValidationException>()
               .WithMessage("*Points must be greater than zero*");
        }

        [Fact]
        public void UpdatePoints_Should_Throw_When_EffectiveTo_Invalid()
        {
            var rewardId = Guid.NewGuid();
            var rp = new RewardPoints(rewardId, 100, null, null);

            var from = new DateTime(2025, 5, 1);
            var to = new DateTime(2025, 4, 1);

            Action act = () => rp.UpdatePoints(200, from, to);

            act.Should().Throw<ValidationException>()
               .WithMessage("*EffectiveTo must be after EffectiveFrom*");
        }
    }
}
