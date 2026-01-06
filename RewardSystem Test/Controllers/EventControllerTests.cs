using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Event;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_Test.Controllers
{
    public class EventControllerTests
    {
        private readonly FakeEventApiService _service;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _service = new FakeEventApiService();
            _controller = new EventController(_service);
        }

        // ===================== DEFINITIONS =====================

        [Fact]
        public async Task GetDefinitions_ShouldReturnOk_WithList()
        {
            // Arrange
            _service.Definitions.Add(new EventDefinitionDto { Id = Guid.NewGuid() });

            // Act
            var result = await _controller.GetDefinitions(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IReadOnlyList<EventDefinitionDto>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task GetDefinitionById_ShouldReturnOk_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new EventDefinitionDto { Id = id };
            _service.Definitions.Add(dto);

            // Act
            var result = await _controller.GetDefinitionById(id, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<EventDefinitionDto>(ok.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetDefinitionById_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.GetDefinitionById(id, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateDefinition_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new EventDefinitionCreateDto();

            // Act
            var result = await _controller.CreateDefinition(dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateDefinition_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var dto = new EventDefinitionCreateDto
            {
                Name = "Hackathon",
                Description = "Desc",
                RewardPoints = 100
            };

            // Act
            var result = await _controller.CreateDefinition(dto, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var def = Assert.IsType<EventDefinitionDto>(created.Value);
            Assert.Equal(dto.Name, def.Name);
            Assert.Equal(nameof(EventController.GetDefinitionById), created.ActionName);
        }

        [Fact]
        public async Task UpdateDefinition_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var id = Guid.NewGuid();
            var dto = new EventDefinitionUpdateDto();

            // Act
            var result = await _controller.UpdateDefinition(id, dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateDefinition_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            _service.Definitions.Add(new EventDefinitionDto { Id = id, Name = "Old" });

            var dto = new EventDefinitionUpdateDto
            {
                Name = "New Name",
                Description = "New",
                RewardPoints = 200
            };

            // Act
            var result = await _controller.UpdateDefinition(id, dto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsType<EventDefinitionDto>(ok.Value);
            Assert.Equal("New Name", updated.Name);
        }

        // ===================== INSTANCES =====================

        [Fact]
        public async Task GetInstances_ShouldReturnOk_WithList()
        {
            // Arrange
            _service.Instances.Add(new EventInstanceDto { Id = Guid.NewGuid() });

            // Act
            var result = await _controller.GetInstances(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IReadOnlyList<EventInstanceDto>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task CreateInstance_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("EventDefinitionId", "Required");
            var dto = new EventInstanceCreateDto();

            // Act
            var result = await _controller.CreateInstance(dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateInstance_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var dto = new EventInstanceCreateDto
            {
                EventDefinitionId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            // Act
            var result = await _controller.CreateInstance(dto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var instance = Assert.IsType<EventInstanceDto>(ok.Value);
            Assert.Equal(dto.EventDefinitionId, instance.EventDefinitionId);
        }

        // ===================== RULES =====================

        [Fact]
        public async Task GetRules_ShouldReturnOk_WithList()
        {
            // Arrange
            var defId = Guid.NewGuid();
            _service.Rules.Add(new EventRewardRuleDto { Id = Guid.NewGuid(), EventDefinitionId = defId });

            // Act
            var result = await _controller.GetRules(defId, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IReadOnlyList<EventRewardRuleDto>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task CreateRule_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Condition", "Required");
            var dto = new EventRewardRuleCreateDto();

            // Act
            var result = await _controller.CreateRule(dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateRule_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var dto = new EventRewardRuleCreateDto
            {
                EventDefinitionId = Guid.NewGuid(),
                Condition = "Rank = 1",
                Points = 100
            };

            // Act
            var result = await _controller.CreateRule(dto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var rule = Assert.IsType<EventRewardRuleDto>(ok.Value);
            Assert.Equal(dto.Points, rule.Points);
        }
    }

    // ========================================================
    //  Fake IEventApiService – sirf tests ke liye simple memory
    // ========================================================
    public sealed class FakeEventApiService : IEventApiService
    {
        public List<EventDefinitionDto> Definitions { get; } = new();
        public List<EventInstanceDto> Instances { get; } = new();
        public List<EventRewardRuleDto> Rules { get; } = new();

        // -------------- Definitions --------------

        public Task<EventDefinitionDto?> GetDefinitionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var def = Definitions.Find(d => d.Id == id);
            return Task.FromResult<EventDefinitionDto?>(def);
        }

        public Task<IReadOnlyList<EventDefinitionDto>> ListDefinitionsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<EventDefinitionDto>>(Definitions);

        public Task<EventDefinitionDto> CreateDefinitionAsync(EventDefinitionCreateDto dto, CancellationToken cancellationToken = default)
        {
            var def = new EventDefinitionDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                RewardPoints = dto.RewardPoints
            };

            Definitions.Add(def);
            return Task.FromResult(def);
        }

        public Task<EventDefinitionDto?> UpdateDefinitionAsync(Guid id, EventDefinitionUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var def = Definitions.Find(d => d.Id == id);
            if (def == null) return Task.FromResult<EventDefinitionDto?>(null);

            def.Name = dto.Name;
            def.Description = dto.Description;
            def.RewardPoints = dto.RewardPoints;

            return Task.FromResult<EventDefinitionDto?>(def);
        }

        // -------------- Instances --------------

        public Task<EventInstanceDto> CreateInstanceAsync(EventInstanceCreateDto dto, CancellationToken cancellationToken = default)
        {
            var instance = new EventInstanceDto
            {
                Id = Guid.NewGuid(),
                EventDefinitionId = dto.EventDefinitionId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            Instances.Add(instance);
            return Task.FromResult(instance);
        }

        public Task<IReadOnlyList<EventInstanceDto>> ListInstancesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<EventInstanceDto>>(Instances);

        // -------------- Rules --------------

        public Task<EventRewardRuleDto> CreateRewardRuleAsync(EventRewardRuleCreateDto dto, CancellationToken cancellationToken = default)
        {
            var rule = new EventRewardRuleDto
            {
                Id = Guid.NewGuid(),
                EventDefinitionId = dto.EventDefinitionId,
                Condition = dto.Condition,
                Points = dto.Points
            };

            Rules.Add(rule);
            return Task.FromResult(rule);
        }

        public Task<IReadOnlyList<EventRewardRuleDto>> ListRewardRulesAsync(Guid eventDefinitionId, CancellationToken cancellationToken = default)
        {
            var list = Rules.FindAll(r => r.EventDefinitionId == eventDefinitionId);
            return Task.FromResult<IReadOnlyList<EventRewardRuleDto>>(list);
        }
    }
}
