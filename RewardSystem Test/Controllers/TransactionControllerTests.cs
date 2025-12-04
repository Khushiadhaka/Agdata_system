using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Transaction;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_API.Tests.Controllers
{
    public class TransactionControllerTests
    {
        private readonly Mock<ITransactionApiService> _transactionServiceMock;
        private readonly TransactionController _controller;

        public TransactionControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionApiService>();
            _controller = new TransactionController(_transactionServiceMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var dto = new TransactionCreateDto();
            _controller.ModelState.AddModelError("Field", "Error");

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _transactionServiceMock.Verify(
                s => s.CreateAsync(It.IsAny<TransactionCreateDto>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithCreatedTransaction()
        {
            // Arrange
            var dto = new TransactionCreateDto();
            var created = new TransactionDto();

            _transactionServiceMock
                .Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(created);

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            // NOTE: Attribute says 201, but controller uses Ok(...)
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(created, okResult.Value);

            _transactionServiceMock.Verify(
                s => s.CreateAsync(dto, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}

