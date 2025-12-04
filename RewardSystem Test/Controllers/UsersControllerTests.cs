using Microsoft.AspNetCore.Mvc;
using Moq;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.User;
using RewardSystem_API.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace RewardSystem_Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserApiService> _userApiMock;

        public UsersControllerTests()
        {
            _userApiMock = new Mock<IUserApiService>();
        }

        private UsersController CreateSut() => new UsersController(_userApiMock.Object);

        // ---------------- USERS ----------------

        [Fact]
        public async Task GetAll_ReturnsOk_WithList()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = Guid.NewGuid(), Name = "Test User" }
            };

            _userApiMock
                .Setup(s => s.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var controller = CreateSut();

            // Act
            var result = await controller.GetAll(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IReadOnlyList<UserDto>>(ok.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetById_WhenUserExists_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UserDto { Id = id, Name = "User" };

            _userApiMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var controller = CreateSut();

            // Act
            var result = await controller.GetById(id, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserDto>(ok.Value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task GetById_WhenUserNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userApiMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto?)null);

            var controller = CreateSut();

            // Act
            var result = await controller.GetById(id, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateSut();
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(new UserCreateDto(), CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_WhenValid_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new UserCreateDto { Name = "New User" };
            var createdDto = new UserDto { Id = Guid.NewGuid(), Name = "New User" };

            _userApiMock
                .Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDto);

            var controller = CreateSut();

            // Act
            var result = await controller.Create(createDto, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UsersController.GetById), created.ActionName);
            Assert.Equal(createdDto, created.Value);
        }

        [Fact]
        public async Task Update_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateSut();
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Update(Guid.NewGuid(), new UserUpdateDto(), CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_WhenUserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userApiMock
                .Setup(s => s.UpdateAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<UserUpdateDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto?)null);

            var controller = CreateSut();

            // Act
            var result = await controller.Update(Guid.NewGuid(), new UserUpdateDto(), CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_WhenValid_ReturnsOkWithUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UserUpdateDto { Name = "Updated" };
            var updatedDto = new UserDto { Id = id, Name = "Updated" };

            _userApiMock
                .Setup(s => s.UpdateAsync(id, updateDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedDto);

            var controller = CreateSut();

            // Act
            var result = await controller.Update(id, updateDto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserDto>(ok.Value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task Delete_Always_ReturnsNoContent_AndCallsService()
        {
            // Arrange
            var id = Guid.NewGuid();
            var controller = CreateSut();

            // Act
            var result = await controller.Delete(id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _userApiMock.Verify(s => s.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------- PROFILE ----------------

        [Fact]
        public async Task GetProfile_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            _userApiMock
                .Setup(s => s.GetProfileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfileDto?)null);

            var controller = CreateSut();

            // Act
            var result = await controller.GetProfile(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetProfile_WhenFound_ReturnsOk()
        {
            // Arrange
            var profile = new UserProfileDto { UserId = Guid.NewGuid() };

            _userApiMock
                .Setup(s => s.GetProfileAsync(profile.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);

            var controller = CreateSut();

            // Act
            var result = await controller.GetProfile(profile.UserId, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserProfileDto>(ok.Value);
            Assert.Equal(profile.UserId, value.UserId);
        }

        [Fact]
        public async Task UpsertProfile_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateSut();
            controller.ModelState.AddModelError("PhoneNumber", "Required");

            // Act
            var result = await controller.UpsertProfile(
                Guid.NewGuid(),
                new UserProfileCreateDto(),
                CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpsertProfile_WhenValid_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var createDto = new UserProfileCreateDto { PhoneNumber = "123" };
            var profile = new UserProfileDto { UserId = id };

            _userApiMock
                .Setup(s => s.UpsertProfileAsync(id, createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);

            var controller = CreateSut();

            // Act
            var result = await controller.UpsertProfile(id, createDto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserProfileDto>(ok.Value);
            Assert.Equal(id, value.UserId);
        }

        // ---------------- ACCOUNT ----------------

        [Fact]
        public async Task GetAccount_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            _userApiMock
                .Setup(s => s.GetAccountAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserAccountDto?)null);

            var controller = CreateSut();

            // Act
            var result = await controller.GetAccount(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAccount_WhenFound_ReturnsOk()
        {
            // Arrange
            var account = new UserAccountDto { UserId = Guid.NewGuid(), Points = 100 };

            _userApiMock
                .Setup(s => s.GetAccountAsync(account.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            var controller = CreateSut();

            // Act
            var result = await controller.GetAccount(account.UserId, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserAccountDto>(ok.Value);
            Assert.Equal(account.UserId, value.UserId);
            Assert.Equal(100, value.Points);
        }

        [Fact]
        public async Task OperateOnAccount_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateSut();
            controller.ModelState.AddModelError("Operation", "Required");

            // Act
            var result = await controller.OperateOnAccount(
                Guid.NewGuid(),
                new UserAccountOperationDto(),
                CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task OperateOnAccount_WhenServiceReturnsNull_ReturnsBadRequest()
        {
            // Arrange
            _userApiMock
                .Setup(s => s.ApplyOperationAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<UserAccountOperationDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserAccountDto?)null);

            var controller = CreateSut();

            // Act
            var result = await controller.OperateOnAccount(
                Guid.NewGuid(),
                new UserAccountOperationDto(),
                CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task OperateOnAccount_WhenSuccess_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var opDto = new UserAccountOperationDto { Operation = "Add", Points = 10 };
            var account = new UserAccountDto { UserId = id, Points = 110 };

            _userApiMock
                .Setup(s => s.ApplyOperationAsync(id, opDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            var controller = CreateSut();

            // Act
            var result = await controller.OperateOnAccount(id, opDto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserAccountDto>(ok.Value);
            Assert.Equal(110, value.Points);
        }
    }
}

