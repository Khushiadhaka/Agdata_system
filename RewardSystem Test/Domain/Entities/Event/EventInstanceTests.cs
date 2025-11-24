using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using Xunit;

namespace RewardSystem_Test.Events
{
    public sealed class EventInstanceTests
    {
        [Fact]
        public void Ctor_Should_Create_Instance_With_Valid_Data()
        {
            var defId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddHours(2);

            var instance = new EventInstance(defId, start, end);

            instance.EventDefinitionId.Should().Be(defId);
            instance.StartTime.Should().Be(start);
            instance.EndTime.Should().Be(end);
            instance.IsCompleted.Should().BeFalse();
            instance.IsCancelled.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Should_Throw_When_End_Before_Start()
        {
            var defId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddMinutes(-5);

            Action act = () => new EventInstance(defId, start, end);

            act.Should().Throw<ValidationException>()
               .WithMessage("*End time must be after start time*");
        }

        [Fact]
        public void AssignWinner_Should_Set_Winner_And_Rank()
        {
            var inst = new EventInstance(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            var userId = Guid.NewGuid();

            inst.AssignWinner(userId, 1);

            inst.WinnerUserId.Should().Be(userId);
            inst.Rank.Should().Be(1);
        }

        [Fact]
        public void MarkCompleted_Should_Set_IsCompleted_True()
        {
            var inst = new EventInstance(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            inst.MarkCompleted();

            inst.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void Cancel_Should_Set_IsCancelled_True()
        {
            var inst = new EventInstance(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            inst.Cancel();

            inst.IsCancelled.Should().BeTrue();
        }

        [Fact]
        public void ExtendEndTime_Should_Update_EndTime_When_Valid()
        {
            var inst = new EventInstance(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            var newEnd = inst.EndTime.AddHours(1);

            inst.ExtendEndTime(newEnd);

            inst.EndTime.Should().Be(newEnd);
        }

        [Fact]
        public void ExtendEndTime_Should_Throw_When_NewEnd_Not_Later()
        {
            var inst = new EventInstance(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            Action act = () => inst.ExtendEndTime(inst.EndTime.AddMinutes(-10));

            act.Should().Throw<ValidationException>()
               .WithMessage("*later than current end time*");
        }
    }
}

