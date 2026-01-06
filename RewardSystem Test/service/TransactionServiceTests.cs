using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _txRepo = new();
        private readonly Mock<IUserAccountRepository> _accRepo = new();
        private readonly Mock<IPointsTransactionRepository> _pointsRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private TransactionService CreateSut() =>
            new(_txRepo.Object, _accRepo.Object, _pointsRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_InvalidAmount_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(Guid.NewGuid(), null, 0m, 0, TransactionType.Credit);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Amount must be positive*");
        }

        [Fact]
        public async Task CreateAsync_WithPoints_AddsPointsAndPointTransaction()
        {
            var sut = CreateSut();
            var uid = Guid.NewGuid();
            var acc = new UserAccount(uid);
            typeof(UserAccount).GetProperty(nameof(UserAccount.Status))!.SetValue(acc, AccountStatus.Active);

            _accRepo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(acc);

            var tx = await sut.CreateAsync(uid, null, 100m, 50, TransactionType.Credit);

            acc.Points.Should().Be(50);
            _pointsRepo.Verify(r => r.AddAsync(It.IsAny<Rewardsystem_Domain.Domain.Entities.Reward.PointsTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
