using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Redemption
{
    public class RedemptionProcessTests
    {
        [Fact]
        public void Ctor_Should_Create_When_Valid()
        {
            var id = Guid.NewGuid();

            var rp = new RedemptionProcess(id, 100);

            rp.RedemptionId.Should().Be(id);
            rp.PointsUsed.Should().Be(100);
            rp.Status.Should().Be(RedemptionStatus.Pending);
        }

        [Fact]
        public void Ctor_Should_Throw_When_Id_Empty()
        {
            Action act = () => new RedemptionProcess(Guid.Empty, 100);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Approve_Should_Set_Status()
        {
            var rp = new RedemptionProcess(Guid.NewGuid(), 100);

            rp.Approve();

            rp.Status.Should().Be(RedemptionStatus.Approved);
        }

        [Fact]
        public void Reject_Should_Set_Status()
        {
            var rp = new RedemptionProcess(Guid.NewGuid(), 100);

            rp.Reject();

            rp.Status.Should().Be(RedemptionStatus.Rejected);
        }

        [Fact]
        public void MarkCompleted_Should_Set_Status_When_Approved()
        {
            var rp = new RedemptionProcess(Guid.NewGuid(), 100);
            rp.Approve();

            rp.MarkCompleted();

            rp.Status.Should().Be(RedemptionStatus.Completed);
        }

        [Fact]
        public void Cancel_Should_Set_Status_Unless_Completed()
        {
            var rp = new RedemptionProcess(Guid.NewGuid(), 100);

            rp.Cancel();

            rp.Status.Should().Be(RedemptionStatus.Cancelled);
        }
    }
}

