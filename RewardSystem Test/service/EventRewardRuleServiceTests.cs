using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class EventRewardRuleServiceTests
    {
        private readonly Mock<IEventRewardRuleRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private EventRewardRuleService CreateSut() =>
            new(_repo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_InvalidCondition_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(Guid.NewGuid(), " ", 10);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Condition required*");
        }

        [Fact]
        public async Task CreateAsync_Valid_AddsAndSaves()
        {
            var sut = CreateSut();
            var id = Guid.NewGuid();

            var rule = await sut.CreateAsync(id, "Rank=1", 100);

            rule.EventDefinitionId.Should().Be(id);
            _repo.Verify(r => r.AddAsync(It.IsAny<EventRewardRule>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
