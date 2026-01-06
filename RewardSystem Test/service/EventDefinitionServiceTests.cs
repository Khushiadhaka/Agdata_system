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
    public class EventDefinitionServiceTests
    {
        private readonly Mock<IEventDefinitionRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private EventDefinitionService CreateSut() =>
            new(_repo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_WhenNameEmpty_ThrowsValidation()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(" ", "desc", 10);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Name required*");
        }

        [Fact]
        public async Task CreateAsync_Valid_AddsAndSaves()
        {
            var sut = CreateSut();

            var result = await sut.CreateAsync("Hackathon", "desc", 100);

            result.Name.Should().Be("Hackathon");
            _repo.Verify(r => r.AddAsync(It.IsAny<EventDefinition>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_Throws()
        {
            var sut = CreateSut();
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((EventDefinition?)null);

            Func<Task> act = () => sut.UpdateAsync(Guid.NewGuid(), "n", "d", 10);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*not found*");
        }
    }
}
