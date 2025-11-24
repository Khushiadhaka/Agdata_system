using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Implementations;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Services
{
    public sealed class RewardServiceTests
    {
        private readonly Mock<IRewardRepository> _rewardRepo;
        private readonly Mock<IRewardPointsRepository> _rewardPointsRepo;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly RewardService _service;

        public RewardServiceTests()
        {
            _rewardRepo = new Mock<IRewardRepository>();
            _rewardPointsRepo = new Mock<IRewardPointsRepository>();
            _uow = new Mock<IUnitOfWork>();

            // IUnitOfWork.SaveChangesAsync -> Task<int>
            _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _service = new RewardService(
                _rewardRepo.Object,
                _rewardPointsRepo.Object,
                _uow.Object);
        }

        // ---------------------------------------------------------
        // CreateRewardAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task CreateRewardAsync_Should_Create_And_Save_Reward()
        {
            // Arrange
            var type = default(RewardType);

            // Act
            var reward = await _service.CreateRewardAsync(
                "Best Performer",
                "Monthly reward",
                type,
                CancellationToken.None);

            // Assert
            reward.Should().NotBeNull();
            reward.Name.Should().Be("Best Performer");
            reward.Description.Should().Be("Monthly reward");
            reward.Type.Should().Be(type);
            reward.IsActive.Should().BeTrue();

            _rewardRepo.Verify(
                r => r.AddAsync(It.IsAny<Reward>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uow.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // GetRewardByIdAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetRewardByIdAsync_Should_Throw_When_Id_Empty()
        {
            Func<Task> act = async () =>
                await _service.GetRewardByIdAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task GetRewardByIdAsync_Should_Return_Reward_From_Repository()
        {
            var id = Guid.NewGuid();
            var type = default(RewardType);
            var reward = new Reward("R1", "D1", type);

            _rewardRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            var result = await _service.GetRewardByIdAsync(id, CancellationToken.None);

            result.Should().Be(reward);

            _rewardRepo.Verify(
                r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // GetAllRewardsAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetAllRewardsAsync_Should_Return_All_Rewards()
        {
            var type = default(RewardType);

            var list = new[]
            {
                new Reward("R1", "D1", type),
                new Reward("R2", "D2", type)
            };

            _rewardRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(list);

            var result = await _service.GetAllRewardsAsync(CancellationToken.None);

            result.Should().HaveCount(2);
            _rewardRepo.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // UpdateRewardAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task UpdateRewardAsync_Should_Throw_When_Id_Empty()
        {
            var type = default(RewardType);

            Func<Task> act = async () =>
                await _service.UpdateRewardAsync(
                    Guid.Empty,
                    "New",
                    "New desc",
                    type,
                    CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task UpdateRewardAsync_Should_Throw_When_Reward_Not_Found()
        {
            _rewardRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Reward?)null);

            var type = default(RewardType);

            Func<Task> act = async () =>
                await _service.UpdateRewardAsync(
                    Guid.NewGuid(),
                    "New",
                    "New desc",
                    type,
                    CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateRewardAsync_Should_Update_And_Save()
        {
            var id = Guid.NewGuid();
            var type = default(RewardType);
            var reward = new Reward("Old", "Old desc", type);

            _rewardRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            var newType = (RewardType)1; // koi dusra enum value

            await _service.UpdateRewardAsync(
                id,
                "New",
                "New desc",
                newType,
                CancellationToken.None);

            reward.Name.Should().Be("New");
            reward.Description.Should().Be("New desc");
            reward.Type.Should().Be(newType);

            _rewardRepo.Verify(r => r.Update(reward), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // DeactivateRewardAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task DeactivateRewardAsync_Should_Throw_When_Id_Empty()
        {
            Func<Task> act = async () =>
                await _service.DeactivateRewardAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task DeactivateRewardAsync_Should_Throw_When_Reward_Not_Found()
        {
            _rewardRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Reward?)null);

            Func<Task> act = async () =>
                await _service.DeactivateRewardAsync(Guid.NewGuid(), CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task DeactivateRewardAsync_Should_Deactivate_And_Save()
        {
            var id = Guid.NewGuid();
            var type = default(RewardType);
            var reward = new Reward("R1", "D1", type);

            _rewardRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            await _service.DeactivateRewardAsync(id, CancellationToken.None);

            reward.IsActive.Should().BeFalse();

            _rewardRepo.Verify(r => r.Update(reward), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // ConfigureRewardPointsAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task ConfigureRewardPointsAsync_Should_Throw_When_RewardId_Empty()
        {
            Func<Task> act = async () =>
                await _service.ConfigureRewardPointsAsync(
                    Guid.Empty,
                    100,
                    null,
                    null,
                    CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ConfigureRewardPointsAsync_Should_Throw_When_Reward_Not_Found()
        {
            _rewardRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Reward?)null);

            Func<Task> act = async () =>
                await _service.ConfigureRewardPointsAsync(
                    Guid.NewGuid(),
                    100,
                    null,
                    null,
                    CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task ConfigureRewardPointsAsync_Should_Create_New_Config_When_Not_Exists()
        {
            var rewardId = Guid.NewGuid();
            var type = default(RewardType);
            var reward = new Reward("R1", "D1", type);

            _rewardRepo.Setup(r => r.GetByIdAsync(rewardId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            _rewardPointsRepo.Setup(r => r.GetByRewardIdAsync(rewardId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((RewardPoints?)null);

            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 12, 31);

            var pointsConfig = await _service.ConfigureRewardPointsAsync(
                rewardId,
                200,
                from,
                to,
                CancellationToken.None);

            pointsConfig.RewardId.Should().Be(rewardId);
            pointsConfig.Points.Should().Be(200);
            pointsConfig.EffectiveFrom.Should().Be(from);
            pointsConfig.EffectiveTo.Should().Be(to);

            _rewardPointsRepo.Verify(
                r => r.AddAsync(It.IsAny<RewardPoints>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ConfigureRewardPointsAsync_Should_Update_Existing_Config_When_Exists()
        {
            var rewardId = Guid.NewGuid();
            var type = default(RewardType);
            var reward = new Reward("R1", "D1", type);

            var existing = new RewardPoints(rewardId, 100, null, null);

            _rewardRepo.Setup(r => r.GetByIdAsync(rewardId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(reward);

            _rewardPointsRepo.Setup(r => r.GetByRewardIdAsync(rewardId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existing);

            var from = new DateTime(2025, 2, 1);
            var to = new DateTime(2025, 12, 31);

            var result = await _service.ConfigureRewardPointsAsync(
                rewardId,
                300,
                from,
                to,
                CancellationToken.None);

            result.Should().Be(existing);
            existing.Points.Should().Be(300);
            existing.EffectiveFrom.Should().Be(from);
            existing.EffectiveTo.Should().Be(to);

            _rewardPointsRepo.Verify(r => r.Update(existing), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

