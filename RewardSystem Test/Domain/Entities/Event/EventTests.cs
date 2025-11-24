using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using Xunit;

namespace RewardSystem_Test.Events
{
    public sealed class EventTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Event_With_Valid_Data()
        {
            // Arrange
            var name = "Quiz Competition";
            var description = "College level quiz";
            var date = DateTime.UtcNow.Date.AddDays(1);

            // Act
            var ev = new Event(name, description, date);

            // Assert
            ev.Name.Should().Be(name);
            ev.Description.Should().Be(description);
            ev.ScheduledDate.Should().Be(date.Date);
            ev.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_ValidationException_When_Name_Is_Empty()
        {
            // Arrange
            var date = DateTime.UtcNow.Date.AddDays(1);

            // Act
            Action act = () => new Event("", "desc", date);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*name cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_ValidationException_When_Date_In_Past()
        {
            // Arrange
            var date = DateTime.UtcNow.Date.AddDays(-1);

            // Act
            Action act = () => new Event("Name", "desc", date);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Scheduled date cannot be in the past*");
        }

        [Fact]
        public void Update_Should_Update_Name_And_Description_When_Active()
        {
            // Arrange
            var ev = new Event("Old", "Old desc", DateTime.UtcNow.Date.AddDays(1));

            // Act
            ev.Update("New", "New desc");

            // Assert
            ev.Name.Should().Be("New");
            ev.Description.Should().Be("New desc");
        }

        [Fact]
        public void Update_Should_Throw_When_Event_Is_Inactive()
        {
            // Arrange
            var ev = new Event("Old", "Old desc", DateTime.UtcNow.Date.AddDays(1));
            ev.Deactivate();

            // Act
            Action act = () => ev.Update("New", "New desc");

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive event*");
        }

        [Fact]
        public void Reschedule_Should_Change_Date_When_Valid()
        {
            // Arrange
            var ev = new Event("Name", "Desc", DateTime.UtcNow.Date.AddDays(1));
            var newDate = DateTime.UtcNow.Date.AddDays(3);

            // Act
            ev.Reschedule(newDate);

            // Assert
            ev.ScheduledDate.Should().Be(newDate.Date);
        }

        [Fact]
        public void Reschedule_Should_Throw_When_NewDate_In_Past()
        {
            // Arrange
            var ev = new Event("Name", "Desc", DateTime.UtcNow.Date.AddDays(1));
            var newDate = DateTime.UtcNow.Date.AddDays(-1);

            // Act
            Action act = () => ev.Reschedule(newDate);

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            // Arrange
            var ev = new Event("Name", "Desc", DateTime.UtcNow.Date.AddDays(1));

            // Act
            ev.Deactivate();

            // Assert
            ev.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_Should_Throw_When_Already_Inactive()
        {
            // Arrange
            var ev = new Event("Name", "Desc", DateTime.UtcNow.Date.AddDays(1));
            ev.Deactivate();

            // Act
            Action act = () => ev.Deactivate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already inactive*");
        }
    }
}
