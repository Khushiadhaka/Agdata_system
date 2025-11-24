using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Redemption
{
    public class RedemptionRequestTests
    {
        [Fact]
        public void Ctor_Should_Create_When_Valid()
        {
            var user = Guid.NewGuid();
            var product = Guid.NewGuid();

            var req = new RedemptionRequest(user, product, 200);

            req.UserId.Should().Be(user);
            req.ProductId.Should().Be(product);
            req.PointsUsed.Should().Be(200);
            req.Status.Should().Be(RedemptionStatus.Pending);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_Should_Throw_When_UserId_Empty(string _)
        {
            Action act = () => new RedemptionRequest(Guid.Empty, Guid.NewGuid(), 100);

            act.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_Should_Throw_When_ProductId_Empty(string _)
        {
            Action act = () => new RedemptionRequest(Guid.NewGuid(), Guid.Empty, 100);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Points_Not_Positive()
        {
            Action act = () => new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 0);

            act.Should().Throw<ValidationException>();
        }


        [Fact]
        public void UpdatePoints_Should_Update_When_Pending()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            r.UpdatePoints(300);

            r.PointsUsed.Should().Be(300);
        }

        [Fact]
        public void UpdatePoints_Should_Throw_When_Not_Pending()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            r.Approve();

            Action act = () => r.UpdatePoints(200);

            act.Should().Throw<BusinessRuleException>();
        }

        [Fact]
        public void Approve_Should_Set_Status()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            r.Approve();

            r.Status.Should().Be(RedemptionStatus.Approved);
        }

        [Fact]
        public void Reject_Should_Set_Status()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            r.Reject();

            r.Status.Should().Be(RedemptionStatus.Rejected);
        }

        [Fact]
        public void MarkCompleted_Should_Set_Status_When_Approved()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            r.Approve();
            r.MarkCompleted();

            r.Status.Should().Be(RedemptionStatus.Completed);
        }

        [Fact]
        public void MarkCompleted_Should_Throw_When_Not_Approved()
        {
            var r = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 100);

            Action act = () => r.MarkCompleted();

            act.Should().Throw<BusinessRuleException>();
        }
    }
}

