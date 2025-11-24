using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Rewards
{
    public sealed class RewardTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Reward_With_Valid_Data()
        {
            // Arrange
            var name = "Employee Reward";
            var desc = "Some reward description";

            // koi bhi valid enum value (default = 0)
            var type = default(RewardType);

            // Act
            var reward = new Reward(name, desc, type);

            // Assert
            reward.Name.Should().Be(name);
            reward.Description.Should().Be(desc);
            reward.Type.Should().Be(type);
            reward.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            var type = default(RewardType);

            // Act
            Action act = () => new Reward("", "desc", type);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Reward name cannot be empty*");
        }

        [Fact]
        public void Update_Should_Change_Fields_When_Active()
        {
            // Arrange
            var initialType = default(RewardType);
            var updatedType = (RewardType)1; // koi dusra enum value (named ho ya na ho, compile ho jayega)

            var reward = new Reward("Old", "Old desc", initialType);

            // Act
            reward.Update("New", "New desc", updatedType);

            // Assert
            reward.Name.Should().Be("New");
            reward.Description.Should().Be("New desc");
            reward.Type.Should().Be(updatedType);
        }

        [Fact]
        public void Update_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Old", "Old desc", type);

            // Act
            Action act = () => reward.Update("", "New desc", type);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Reward name cannot be empty*");
        }

        [Fact]
        public void Update_Should_Throw_When_Reward_Is_Inactive()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Old", "Old desc", type);
            reward.Deactivate();

            // Act
            Action act = () => reward.Update("New", "New desc", type);

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive reward*");
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Name", "Desc", type);

            // Act
            reward.Deactivate();

            // Assert
            reward.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_Should_Throw_When_Already_Inactive()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Name", "Desc", type);
            reward.Deactivate();

            // Act
            Action act = () => reward.Deactivate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already inactive*");
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True_When_Inactive()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Name", "Desc", type);
            reward.Deactivate();

            // Act
            reward.Activate();

            // Assert
            reward.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Activate_Should_Throw_When_Already_Active()
        {
            // Arrange
            var type = default(RewardType);
            var reward = new Reward("Name", "Desc", type);

            // Act
            Action act = () => reward.Activate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already active*");
        }
    }
}
