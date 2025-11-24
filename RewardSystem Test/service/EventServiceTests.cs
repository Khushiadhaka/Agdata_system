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
using Rewardsystem_Domain.Domain.Entities.Event;
using Xunit;

namespace RewardSystem_Test.Services
{
    public sealed class EventServiceTests
    {
        private readonly Mock<IEventDefinitionRepository> _defRepoMock;
        private readonly Mock<IEventInstanceRepository> _instRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _defRepoMock = new Mock<IEventDefinitionRepository>();
            _instRepoMock = new Mock<IEventInstanceRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            // IUnitOfWork.SaveChangesAsync returns Task<int>
            _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            _service = new EventService(
                _defRepoMock.Object,
                _instRepoMock.Object,
                _uowMock.Object);
        }

        [Fact]
        public async Task CreateEventDefinitionAsync_Should_Save_Definition_And_Return_It()
        {
            // Act
            var result = await _service.CreateEventDefinitionAsync(
                "Test Event",
                "Sample description",
                100,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Event");
            result.RewardPoints.Should().Be(100);

            _defRepoMock.Verify(
                x => x.AddAsync(It.IsAny<EventDefinition>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetEventDefinitionByIdAsync_Should_Throw_When_Id_Is_Empty()
        {
            // Act
            Func<Task> act = async () =>
                await _service.GetEventDefinitionByIdAsync(Guid.Empty, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*EventDefinitionId cannot be empty*");
        }

        [Fact]
        public async Task GetEventDefinitionByIdAsync_Should_Return_Definition_From_Repository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var def = new EventDefinition("Name", "Desc", 10);

            _defRepoMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(def);

            // Act
            var result = await _service.GetEventDefinitionByIdAsync(id, CancellationToken.None);

            // Assert
            result.Should().Be(def);

            _defRepoMock.Verify(
                x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllEventDefinitionsAsync_Should_Return_All_From_Repository()
        {
            // Arrange
            var list = new List<EventDefinition>
            {
                new EventDefinition("E1", "D1", 10),
                new EventDefinition("E2", "D2", 20)
            };

            _defRepoMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(list);

            // Act
            var result = await _service.GetAllEventDefinitionsAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            _defRepoMock.Verify(
                x => x.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ScheduleEventInstanceAsync_Should_Create_Instance_When_Definition_Exists()
        {
            // Arrange
            var defId = Guid.NewGuid();
            var def = new EventDefinition("Name", "Desc", 10);

            _defRepoMock.Setup(x => x.GetByIdAsync(defId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(def);

            var start = DateTime.UtcNow;
            var end = start.AddHours(1);

            // Act
            var instance = await _service.ScheduleEventInstanceAsync(
                defId,
                start,
                end,
                CancellationToken.None);

            // Assert
            instance.EventDefinitionId.Should().Be(defId);
            instance.StartTime.Should().Be(start);
            instance.EndTime.Should().Be(end);

            _instRepoMock.Verify(
                x => x.AddAsync(It.IsAny<EventInstance>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ScheduleEventInstanceAsync_Should_Throw_When_Definition_Not_Found()
        {
            // Arrange
            var defId = Guid.NewGuid();

            _defRepoMock.Setup(x => x.GetByIdAsync(defId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((EventDefinition?)null);

            // Act
            Func<Task> act = async () => await _service.ScheduleEventInstanceAsync(
                defId,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Event definition not found*");
        }

        [Fact]
        public async Task ScheduleEventInstanceAsync_Should_Throw_When_DefinitionId_Empty()
        {
            // Act
            Func<Task> act = async () => await _service.ScheduleEventInstanceAsync(
                Guid.Empty,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*EventDefinitionId cannot be empty*");
        }

        [Fact]
        public async Task AssignWinnerAsync_Should_Update_Instance_And_Save()
        {
            // Arrange
            var instanceId = Guid.NewGuid();
            var instance = new EventInstance(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1));

            _instRepoMock.Setup(x => x.GetByIdAsync(instanceId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(instance);

            var userId = Guid.NewGuid();

            // Act
            await _service.AssignWinnerAsync(
                instanceId,
                userId,
                1,
                CancellationToken.None);

            // Assert
            instance.WinnerUserId.Should().Be(userId);
            instance.Rank.Should().Be(1);

            _instRepoMock.Verify(
                x => x.Update(instance),
                Times.Once);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AssignWinnerAsync_Should_Throw_When_InstanceId_Empty()
        {
            // Act
            Func<Task> act = async () => await _service.AssignWinnerAsync(
                Guid.Empty,
                Guid.NewGuid(),
                1,
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*EventInstanceId cannot be empty*");
        }

        [Fact]
        public async Task AssignWinnerAsync_Should_Throw_When_Instance_Not_Found()
        {
            // Arrange
            var instanceId = Guid.NewGuid();

            _instRepoMock.Setup(x => x.GetByIdAsync(instanceId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((EventInstance?)null);

            // Act
            Func<Task> act = async () => await _service.AssignWinnerAsync(
                instanceId,
                Guid.NewGuid(),
                1,
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Event instance not found*");
        }
    }
}
