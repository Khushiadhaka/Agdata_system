using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Users
{
    public sealed class UserAccountTests
    {
        [Fact]
        public void Ctor_Should_Create_Account_With_Zero_Points()
        {
            var acc = new UserAccount(Guid.NewGuid());
            acc.Points.Should().Be(0);
            acc.Status.Should().Be(AccountStatus.Active);
        }

        [Fact]
        public void AddPoints_Should_Increase_Points()
        {
            var acc = new UserAccount(Guid.NewGuid());
            acc.AddPoints(10);
            acc.Points.Should().Be(10);
        }

        [Fact]
        public void AddPoints_Should_Throw_When_Not_Active()
        {
            var acc = new UserAccount(Guid.NewGuid());
            acc.Deactivate();

            Action act = () => acc.AddPoints(10);
            act.Should().Throw<BusinessRuleException>();
        }

        [Fact]
        public void DeductPoints_Should_Throw_When_Insufficient()
        {
            var acc = new UserAccount(Guid.NewGuid());
            acc.AddPoints(5);

            Action act = () => acc.DeductPoints(10);
            act.Should().Throw<BusinessRuleException>();
        }
    }
}
