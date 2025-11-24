using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using Xunit;

namespace RewardSystem_Test.Events
{
    public sealed class EventDefinitionTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Definition_With_Valid_Data()
        {
            var def = new EventDefinition("Name", "Desc", 100);

            def.Name.Should().Be("Name");
            def.Description.Should().Be("Desc");
            def.RewardPoints.Should().Be(100);
            def.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Name_Empty()
        {
            Action act = () => new EventDefinition("", "Desc", 100);

            act.Should().Throw<ValidationException>()
               .WithMessage("*name cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_Points_Not_Positive()
        {
            Action act = () => new EventDefinition("Name", "Desc", 0);

            act.Should().Throw<ValidationException>()
               .WithMessage("*greater than zero*");
        }

        [Fact]
        public void Update_Should_Set_Values_When_Active()
        {
            var def = new EventDefinition("Old", "Old", 10);

            def.Update("New", "New", 20);

            def.Name.Should().Be("New");
            def.Description.Should().Be("New");
            def.RewardPoints.Should().Be(20);
        }

        [Fact]
        public void Update_Should_Throw_When_Inactive()
        {
            var def = new EventDefinition("Old", "Old", 10);
            def.Deactivate();

            Action act = () => def.Update("New", "New", 20);

            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive event definition*");
        }

        [Fact]
        public void Deactivate_And_Activate_Work_As_Expected()
        {
            var def = new EventDefinition("Name", "Desc", 10);

            def.Deactivate();
            def.IsActive.Should().BeFalse();

            def.Activate();
            def.IsActive.Should().BeTrue();
        }
    }
}
