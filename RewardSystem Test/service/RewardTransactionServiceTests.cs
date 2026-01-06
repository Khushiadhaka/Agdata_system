using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class RewardTransactionServiceTests
    {
        private readonly Mock<IRewardTransactionRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RewardTransactionService CreateSut() => new(_repo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_InvalidPoints_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                0,
                TransactionType.Credit);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*positive*");
        }

        [Fact]
        public async Task CreateAsync_Valid_AddsAndSaves()
        {
            var sut = CreateSut();

            var rt = await sut.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), 10, TransactionType.Credit);

            _repo.Verify(r => r.AddAsync(It.IsAny<RewardTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
