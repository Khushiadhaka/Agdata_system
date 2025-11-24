using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using Xunit;

namespace RewardSystem_Test.Events
{
    public sealed class EventRewardRuleTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Rule_With_Valid_Data()
        {
            var rule = new EventRewardRule(Guid.NewGuid(), "Top 3", 50);

            rule.EventDefinitionId.Should().NotBeEmpty();
            rule.Condition.Should().Be("Top 3");
            rule.Points.Should().Be(50);
            rule.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Condition_Empty()
        {
            Action act = () => new EventRewardRule(Guid.NewGuid(), "", 50);

            act.Should().Throw<ValidationException>()
               .WithMessage("*Condition cannot be empty*");
        }

        [Fact]
        public void Update_Should_Change_Values_When_Active()
        {
            var rule = new EventRewardRule(Guid.NewGuid(), "Old", 10);

            rule.Update("New", 20);

            rule.Condition.Should().Be("New");
            rule.Points.Should().Be(20);
        }

        [Fact]
        public void Update_Should_Throw_When_Inactive()
        {
            var rule = new EventRewardRule(Guid.NewGuid(), "Old", 10);
            rule.Deactivate();

            Action act = () => rule.Update("New", 20);

            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive reward rule*");
        }

        [Fact]
        public void Activate_And_Deactivate_Work_As_Expected()
        {
            var rule = new EventRewardRule(Guid.NewGuid(), "Cond", 10);

            rule.Deactivate();
            rule.IsActive.Should().BeFalse();

            rule.Activate();
            rule.IsActive.Should().BeTrue();
        }
    }
}

