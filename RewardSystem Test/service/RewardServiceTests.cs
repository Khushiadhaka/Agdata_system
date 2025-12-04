using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RewardSystem_Test.Services
{
    public class RewardServiceTests
    {
        private readonly Mock<IRewardRepository> _rewardRepo = new();
        private readonly Mock<IRewardPointsRepository> _rpRepo = new();
        private readonly Mock<IUserAccountRepository> _accRepo = new();
        private readonly Mock<IRewardTransactionRepository> _rewardTxRepo = new();
        private readonly Mock<IPointsTransactionRepository> _pointsTxRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RewardService CreateSut() =>
            new(_rewardRepo.Object, _rpRepo.Object, _accRepo.Object,
                _rewardTxRepo.Object, _pointsTxRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateRewardAsync_InvalidName_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateRewardAsync(" ", null, RewardType.Generic, 10);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task AwardRewardAsync_InactiveReward_Throws()
        {
            var sut = CreateSut();
            var reward = new Reward("R", null, RewardType.Generic);
            reward.Deactivate();
            var rid = Guid.NewGuid();
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(reward, rid);

            _rewardRepo.Setup(r => r.GetByIdAsync(rid, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            Func<Task> act = () => sut.AwardRewardAsync(rid, Guid.NewGuid(), 10);

            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*Reward inactive*");
        }
    }
}
