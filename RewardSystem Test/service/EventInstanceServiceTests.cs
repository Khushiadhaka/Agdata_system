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
    public class EventInstanceServiceTests
    {
        private readonly Mock<IEventInstanceRepository> _instRepo = new();
        private readonly Mock<IEventDefinitionRepository> _defRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private EventInstanceService CreateSut() =>
            new(_instRepo.Object, _defRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_DefinitionNotFound_ThrowsValidation()
        {
            var sut = CreateSut();
            _defRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((EventDefinition?)null);

            Func<Task> act = () => sut.CreateAsync(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*definition not found*");
        }

        [Fact]
        public async Task CreateAsync_DefinitionInactive_ThrowsBusinessRule()
        {
            var sut = CreateSut();
            var def = new EventDefinition("Test", null, 10);
            def.Deactivate();
            _defRepo.Setup(r => r.GetByIdAsync(def.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(def);

            Func<Task> act = () => sut.CreateAsync(def.Id, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*Definition inactive*");
        }

        [Fact]
        public async Task CreateAsync_Valid_AddsInstanceAndSaves()
        {
            var sut = CreateSut();
            var def = new EventDefinition("Test", null, 10);
            _defRepo.Setup(r => r.GetByIdAsync(def.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(def);

            var inst = await sut.CreateAsync(def.Id, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            inst.EventDefinitionId.Should().Be(def.Id);
            _instRepo.Verify(r => r.AddAsync(It.IsAny<EventInstance>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
