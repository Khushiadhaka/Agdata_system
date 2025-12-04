using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Product;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_API.Tests.Controllers
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryApiService> _inventoryServiceMock;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _inventoryServiceMock = new Mock<IInventoryApiService>();
            _controller = new InventoryController(_inventoryServiceMock.Object);
        }

        #region GetByProduct

        [Fact]
        public async Task GetByProduct_ReturnsNotFound_WhenInventoryIsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _inventoryServiceMock
                .Setup(s => s.GetInventoryAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductInventoryDto?)null);

            // Act
            var result = await _controller.GetByProduct(productId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            dynamic value = notFoundResult.Value!;
            Assert.Equal("Inventory record not found.", (string)value.message);
        }

        [Fact]
        public async Task GetByProduct_ReturnsOk_WhenInventoryExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var inventoryDto = new ProductInventoryDto(); // properties not needed for this test

            _inventoryServiceMock
                .Setup(s => s.GetInventoryAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(inventoryDto);

            // Act
            var result = await _controller.GetByProduct(productId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(inventoryDto, okResult.Value);
        }

        #endregion

        #region UpdateStock

        [Fact]
        public async Task UpdateStock_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new InventoryController.UpdateStockRequest
            {
                QuantityChange = 10
            };

            _controller.ModelState.AddModelError("QuantityChange", "Invalid quantity.");

            // Act
            var result = await _controller.UpdateStock(productId, request, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            // Service should NOT be called when model state is invalid
            _inventoryServiceMock.Verify(
                s => s.UpdateStockAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStock_ReturnsBadRequest_WhenServiceReturnsFalse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new InventoryController.UpdateStockRequest
            {
                QuantityChange = 5
            };

            _inventoryServiceMock
                .Setup(s => s.UpdateStockAsync(productId, request.QuantityChange, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateStock(productId, request, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            dynamic value = badRequest.Value!;
            Assert.Equal("Could not update stock.", (string)value.message);
        }

        [Fact]
        public async Task UpdateStock_ReturnsOk_WhenServiceReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new InventoryController.UpdateStockRequest
            {
                QuantityChange = 5
            };

            _inventoryServiceMock
                .Setup(s => s.UpdateStockAsync(productId, request.QuantityChange, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateStock(productId, request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            dynamic value = okResult.Value!;
            Assert.Equal("Stock updated.", (string)value.message);
        }

        #endregion
    }
}

