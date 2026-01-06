using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Server.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.DTOs.Auth;
using RewardSystem_Application.Interfaces.Auth;
using Xunit;

namespace Api.Server.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenAuthServiceIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AuthController(null!));
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "admin@agdata.com",
                Password = "Admin@123"
            };

            _controller.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _controller.Login(dto, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _authServiceMock.Verify(
                s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Login_ReturnsUnauthorized_WhenTokenIsNullOrWhitespace(string? token)
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "admin@agdata.com",
                Password = "Admin@123"
            };

            _authServiceMock
                .Setup(s => s.LoginAsync(dto.Email, dto.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(token!);

            // Act
            var result = await _controller.Login(dto, CancellationToken.None);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status401Unauthorized, unauthorized.StatusCode);

            dynamic value = unauthorized.Value!;
            Assert.Equal("Invalid email or password.", (string)value.message);
        }

        [Fact]
        public async Task Login_ReturnsOkWithAuthResponse_WhenTokenValid()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "admin@agdata.com",
                Password = "Admin@123"
            };

            const string expectedToken = "fake-jwt-token";

            _authServiceMock
                .Setup(s => s.LoginAsync(dto.Email, dto.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(dto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var response = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.Equal(expectedToken, response.Token);

            _authServiceMock.Verify(
                s => s.LoginAsync(dto.Email, dto.Password, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}

