using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Domain.Transactions
{
    public sealed class TransactionTests
    {
        [Fact]
        public void Ctor_Should_Create_Valid_Transaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var amount = 150.75m;
            var points = 20;
            var type = default(TransactionType);

            // Act
            var tx = new Transaction(userId, productId, amount, points, type);

            // Assert
            tx.UserId.Should().Be(userId);
            tx.ProductId.Should().Be(productId);
            tx.Amount.Should().Be(amount);
            tx.RewardPointsEarned.Should().Be(points);
            tx.Type.Should().Be(type);
            tx.Status.Should().Be(TransactionStatus.Pending);
        }

        [Fact]
        public void Ctor_Should_Allow_Null_ProductId()
        {
            var userId = Guid.NewGuid();
            var type = default(TransactionType);

            var tx = new Transaction(userId, null, 100m, 10, type);

            tx.ProductId.Should().BeNull();
        }

        [Fact]
        public void Ctor_Should_Throw_When_UserId_Empty()
        {
            var type = default(TransactionType);

            Action act = () => new Transaction(Guid.Empty, null, 100m, 10, type);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Amount_Not_Positive()
        {
            var userId = Guid.NewGuid();
            var type = default(TransactionType);

            Action act = () => new Transaction(userId, null, 0m, 10, type);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Ctor_Should_Throw_When_RewardPoints_Negative()
        {
            var userId = Guid.NewGuid();
            var type = default(TransactionType);

            Action act = () => new Transaction(userId, null, 100m, -1, type);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void MarkCompleted_Should_Set_Status_To_Completed()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            tx.MarkCompleted();

            tx.Status.Should().Be(TransactionStatus.Completed);
        }

        [Fact]
        public void MarkCompleted_Should_Throw_When_Already_Completed()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            tx.MarkCompleted();

            Action act = () => tx.MarkCompleted();

            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already completed*");
        }

        [Fact]
        public void MarkFailed_Should_Set_Status_To_Failed_When_Not_Completed()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            tx.MarkFailed();

            tx.Status.Should().Be(TransactionStatus.Failed);
        }

        [Fact]
        public void MarkFailed_Should_Throw_When_Already_Completed()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            tx.MarkCompleted();

            Action act = () => tx.MarkFailed();

            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*Completed transaction cannot be marked as failed*");
        }

        [Fact]
        public void Update_Should_Change_Amount_Points_And_Type()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            var newType = (TransactionType)1;
            tx.Update(200m, 50, newType);

            tx.Amount.Should().Be(200m);
            tx.RewardPointsEarned.Should().Be(50);
            tx.Type.Should().Be(newType);
        }

        [Fact]
        public void Update_Should_Throw_When_Amount_Not_Positive()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            Action act = () => tx.Update(0m, 10, default(TransactionType));

            act.Should().Throw<ValidationException>()
               .WithMessage("*Amount must be positive*");
        }

        [Fact]
        public void Update_Should_Throw_When_RewardPoints_Negative()
        {
            var tx = new Transaction(
                Guid.NewGuid(),
                null,
                100m,
                10,
                default(TransactionType));

            Action act = () => tx.Update(50m, -1, default(TransactionType));

            act.Should().Throw<ValidationException>()
               .WithMessage("*Reward points cannot be negative*");
        }
    }
}

