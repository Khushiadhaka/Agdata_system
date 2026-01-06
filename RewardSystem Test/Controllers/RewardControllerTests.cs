using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Reward;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_API.Tests.Controllers
{
    public class RewardControllerTests
    {
        private readonly Mock<IRewardApiService> _rewardServiceMock;
        private readonly RewardController _controller;

        public RewardControllerTests()
        {
            _rewardServiceMock = new Mock<IRewardApiService>();
            _controller = new RewardController(_rewardServiceMock.Object);
        }

        // ---------- Reward definitions ----------

        [Fact]
        public async Task GetAll_ReturnsOkWithList()
        {
            // Arrange
            var list = new List<RewardDto>
            {
                new RewardDto(),
                new RewardDto()
            };

            _rewardServiceMock
                .Setup(s => s.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(list, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _rewardServiceMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RewardDto?)null);

            // Act
            var result = await _controller.GetById(id, CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

            dynamic value = notFound.Value!;
            Assert.Equal("Reward not found.", (string)value.message);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardDto { Id = id };

            _rewardServiceMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(id, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(dto, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var dto = new RewardCreateDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _rewardServiceMock.Verify(
                s => s.CreateAsync(It.IsAny<RewardCreateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var dto = new RewardCreateDto();
            var createdId = Guid.NewGuid();
            var createdDto = new RewardDto { Id = createdId };

            _rewardServiceMock
                .Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(RewardController.GetById), createdResult.ActionName);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(createdId, createdResult.RouteValues!["id"]);
            Assert.Same(createdDto, createdResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardUpdateDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _rewardServiceMock.Verify(
                s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<RewardUpdateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardUpdateDto();

            _rewardServiceMock
                .Setup(s => s.UpdateAsync(id, dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RewardDto?)null);

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

            dynamic value = notFound.Value!;
            Assert.Equal("Reward not found.", (string)value.message);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardUpdateDto();
            var updatedDto = new RewardDto { Id = id };

            _rewardServiceMock
                .Setup(s => s.UpdateAsync(id, dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedDto);

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(updatedDto, okResult.Value);
        }

        // ---------- Reward points ----------

        [Fact]
        public async Task GetPoints_ReturnsNotFound_WhenNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _rewardServiceMock
                .Setup(s => s.GetRewardPointsAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RewardPointsDto?)null);

            // Act
            var result = await _controller.GetPoints(id, CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

            dynamic value = notFound.Value!;
            Assert.Equal("Reward points not found.", (string)value.message);
        }

        [Fact]
        public async Task GetPoints_ReturnsOk_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var pointsDto = new RewardPointsDto();

            _rewardServiceMock
                .Setup(s => s.GetRewardPointsAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pointsDto);

            // Act
            var result = await _controller.GetPoints(id, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(pointsDto, okResult.Value);
        }

        [Fact]
        public async Task SetPoints_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardPointsCreateDto();
            _controller.ModelState.AddModelError("Points", "Required");

            // Act
            var result = await _controller.SetPoints(id, dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _rewardServiceMock.Verify(
                s => s.SetRewardPointsAsync(It.IsAny<RewardPointsCreateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task SetPoints_ReturnsOk_WhenValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RewardPointsCreateDto();
            var returnedDto = new RewardPointsDto();

            _rewardServiceMock
                .Setup(s => s.SetRewardPointsAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnedDto);

            // Act
            var result = await _controller.SetPoints(id, dto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(returnedDto, okResult.Value);
        }

        // ---------- Reward transactions ----------

        [Fact]
        public async Task GetUserRewardTransactions_ReturnsOkWithList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var list = new List<RewardTransactionDto> { new RewardTransactionDto() };

            _rewardServiceMock
                .Setup(s => s.ListRewardTransactionsByUserAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.GetUserRewardTransactions(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(list, okResult.Value);
        }

        [Fact]
        public async Task CreateRewardTransaction_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var dto = new RewardTransactionCreateDto();
            _controller.ModelState.AddModelError("Amount", "Required");

            // Act
            var result = await _controller.CreateRewardTransaction(dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _rewardServiceMock.Verify(
                s => s.CreateRewardTransactionAsync(It.IsAny<RewardTransactionCreateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateRewardTransaction_ReturnsOk_WhenCreated()
        {
            // Arrange
            var dto = new RewardTransactionCreateDto();
            var created = new RewardTransactionDto();

            _rewardServiceMock
                .Setup(s => s.CreateRewardTransactionAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(created);

            // Act
            var result = await _controller.CreateRewardTransaction(dto, CancellationToken.None);

            // Assert
            // NOTE: Controller uses Ok(...) not CreatedAtAction
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(created, okResult.Value);
        }

        // ---------- Top 3 employees ----------

        [Fact]
        public async Task GetTop3Employees_ReturnsOkWithList()
        {
            // Arrange
            var list = new List<Top3EmployeeRewardDto>
            {
                new Top3EmployeeRewardDto(),
                new Top3EmployeeRewardDto()
            };

            _rewardServiceMock
                .Setup(s => s.GetTop3EmployeesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.GetTop3Employees(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(list, okResult.Value);
        }
    }
}
