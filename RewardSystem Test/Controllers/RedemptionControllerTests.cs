using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Redemption;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_API.Tests.Controllers
{
    public class RedemptionControllerTests
    {
        private readonly Mock<IRedemptionApiService> _redemptionServiceMock;
        private readonly RedemptionController _controller;

        public RedemptionControllerTests()
        {
            _redemptionServiceMock = new Mock<IRedemptionApiService>();
            _controller = new RedemptionController(_redemptionServiceMock.Object);
        }

        // ----- CreateRequest -----

        [Fact]
        public async Task CreateRequest_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var dto = new RedemptionRequestCreateDto();
            _controller.ModelState.AddModelError("Field", "Error");

            // Act
            var result = await _controller.CreateRequest(dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _redemptionServiceMock.Verify(
                s => s.CreateRequestAsync(It.IsAny<RedemptionRequestCreateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateRequest_ReturnsOk_WithCreatedRequest()
        {
            // Arrange
            var dto = new RedemptionRequestCreateDto();
            var created = new RedemptionRequestDto();

            _redemptionServiceMock
                .Setup(s => s.CreateRequestAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(created);

            // Act
            var result = await _controller.CreateRequest(dto, CancellationToken.None);

            // Assert
            // NOTE: Controller uses Ok(...) even though attribute says 201
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(created, okResult.Value);
        }

        // ----- UpdateRequest -----

        [Fact]
        public async Task UpdateRequest_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RedemptionRequestUpdateDto();
            _controller.ModelState.AddModelError("Field", "Error");

            // Act
            var result = await _controller.UpdateRequest(id, dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _redemptionServiceMock.Verify(
                s => s.UpdateRequestAsync(It.IsAny<Guid>(), It.IsAny<RedemptionRequestUpdateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateRequest_ReturnsOk_WithUpdatedRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RedemptionRequestUpdateDto();
            var updated = new RedemptionRequestDto();

            _redemptionServiceMock
                .Setup(s => s.UpdateRequestAsync(id, dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateRequest(id, dto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(updated, okResult.Value);
        }

        // ----- GetRequestsForUser -----

        [Fact]
        public async Task GetRequestsForUser_ReturnsOk_WithList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var list = new List<RedemptionRequestDto> { new RedemptionRequestDto() };

            _redemptionServiceMock
                .Setup(s => s.ListRequestsByUserAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.GetRequestsForUser(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(list, okResult.Value);
        }

        // ----- GetRecordsForUser -----

        [Fact]
        public async Task GetRecordsForUser_ReturnsOk_WithList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var list = new List<RedemptionRecordDto> { new RedemptionRecordDto() };

            _redemptionServiceMock
                .Setup(s => s.ListRecordsByUserAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.GetRecordsForUser(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(list, okResult.Value);
        }
    }
}

