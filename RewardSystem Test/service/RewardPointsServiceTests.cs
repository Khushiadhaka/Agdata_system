using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class RewardPointsServiceTests
    {
        private readonly Mock<IRewardPointsRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RewardPointsService CreateSut() => new(_repo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_InvalidPoints_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(Guid.NewGuid(), 0);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Points must be positive*");
        }

        [Fact]
        public async Task GetLatestForRewardAsync_ReturnsMostRecent()
        {
            var sut = CreateSut();
            var id = Guid.NewGuid();
            var older = new RewardPoints(id, 10, null, null);
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.CreatedAt))!.SetValue(older, DateTime.UtcNow.AddDays(-1));
            var newer = new RewardPoints(id, 20, null, null);
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.CreatedAt))!.SetValue(newer, DateTime.UtcNow);

            _repo.Setup(r => r.GetByRewardIdAsync(id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<RewardPoints> { older, newer });

            var result = await sut.GetLatestForRewardAsync(id);

            result.Should().Be(newer);
        }
    }
}
