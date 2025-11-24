using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Implementations;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Services
{
    public sealed class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IUserAccountRepository> _accRepo;
        private readonly Mock<IUserProfileRepository> _profileRepo;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepo = new Mock<IUserRepository>();
            _accRepo = new Mock<IUserAccountRepository>();
            _profileRepo = new Mock<IUserProfileRepository>();
            _uow = new Mock<IUnitOfWork>();

            // IUnitOfWork.SaveChangesAsync returns Task<int>
            _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _service = new UserService(
                _userRepo.Object,
                _accRepo.Object,
                _profileRepo.Object,
                _uow.Object);
        }

        // ---------------------------------------------------------
        // CreateUserAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task CreateUserAsync_Should_Create_When_Email_Does_Not_Exist()
        {
            // Arrange
            _userRepo.Setup(r => r.GetByEmailAsync(
                                It.IsAny<string>(),
                                It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            // Act
            var user = await _service.CreateUserAsync(
                "Gajendra",
                "gajendra@example.com",
                "EMP001",
                UserRole.Employee,
                CancellationToken.None);

            // Assert
            user.Should().NotBeNull();
            user.Name.Should().Be("Gajendra");
            user.Email.Should().Be("gajendra@example.com");
            user.EmployeeId.Should().Be("EMP001");
            user.Role.Should().Be(UserRole.Employee);

            _userRepo.Verify(
                r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uow.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var existing = new User("Old", "gajendra@example.com", "EMP001", UserRole.Employee);

            _userRepo.Setup(r => r.GetByEmailAsync(
                                It.IsAny<string>(),
                                It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existing);

            // Act
            Func<Task> act = async () => await _service.CreateUserAsync(
                "New",
                "gajendra@example.com",
                "EMP002",
                UserRole.Employee,
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Name_Is_Empty()
        {
            Func<Task> act = async () => await _service.CreateUserAsync(
                "",
                "a@a.com",
                "E1",
                UserRole.Employee,
                CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Name is required*");
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Email_Is_Empty()
        {
            Func<Task> act = async () => await _service.CreateUserAsync(
                "Name",
                "",
                "E1",
                UserRole.Employee,
                CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Email is required*");
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_EmployeeId_Is_Empty()
        {
            Func<Task> act = async () => await _service.CreateUserAsync(
                "Name",
                "a@a.com",
                "",
                UserRole.Employee,
                CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*EmployeeId is required*");
        }

        // ---------------------------------------------------------
        // GetByIdAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.GetByIdAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Id cannot be empty*");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_From_Repository()
        {
            var id = Guid.NewGuid();
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);

            _userRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            var result = await _service.GetByIdAsync(id, CancellationToken.None);

            result.Should().Be(user);

            _userRepo.Verify(
                r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // GetAllAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_Should_Return_Users_From_Repository()
        {
            var list = new[]
            {
                new User("U1", "u1@a.com", "E1", UserRole.Employee),
                new User("U2", "u2@a.com", "E2", UserRole.Admin),
            };

            _userRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(list);

            var result = await _service.GetAllAsync(CancellationToken.None);

            result.Should().HaveCount(2);
            _userRepo.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // UpdateUserAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task UpdateUserAsync_Should_Update_When_User_Exists()
        {
            var id = Guid.NewGuid();
            var user = new User("Old", "old@a.com", "E1", UserRole.Employee);

            _userRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            await _service.UpdateUserAsync(
                id,
                "New",
                "new@a.com",
                UserRole.Admin,
                CancellationToken.None);

            user.Name.Should().Be("New");
            user.Email.Should().Be("new@a.com");
            user.Role.Should().Be(UserRole.Admin);

            _userRepo.Verify(r => r.Update(user), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.UpdateUserAsync(
                    Guid.Empty,
                    "New",
                    "new@a.com",
                    UserRole.Admin,
                    CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Id cannot be empty*");
        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_When_User_Not_Found()
        {
            _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            Func<Task> act = async () =>
                await _service.UpdateUserAsync(
                    Guid.NewGuid(),
                    "New",
                    "new@a.com",
                    UserRole.Admin,
                    CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*User not found*");
        }

        // ---------------------------------------------------------
        // DeleteUserAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task DeleteUserAsync_Should_SoftDelete_When_User_Exists()
        {
            var id = Guid.NewGuid();
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);

            _userRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            await _service.DeleteUserAsync(id, CancellationToken.None);

            user.IsDeleted.Should().BeTrue();

            _userRepo.Verify(r => r.Update(user), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.DeleteUserAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Id cannot be empty*");
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Throw_When_User_Not_Found()
        {
            _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            Func<Task> act = async () =>
                await _service.DeleteUserAsync(Guid.NewGuid(), CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*User not found*");
        }

        // ---------------------------------------------------------
        // CreateUserAccountAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task CreateUserAccountAsync_Should_Create_When_User_Exists_And_No_Account()
        {
            var userId = Guid.NewGuid();
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);

            _userRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            _accRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((UserAccount?)null);

            var account = await _service.CreateUserAccountAsync(userId, CancellationToken.None);

            account.Should().NotBeNull();
            account.UserId.Should().Be(userId);

            _accRepo.Verify(r => r.AddAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserAccountAsync_Should_Throw_When_UserId_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.CreateUserAccountAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*UserId cannot be empty*");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Should_Throw_When_User_Not_Found()
        {
            _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            Func<Task> act = async () =>
                await _service.CreateUserAccountAsync(Guid.NewGuid(), CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*User not found*");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Should_Throw_When_Account_Already_Exists()
        {
            var userId = Guid.NewGuid();
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);
            var account = new UserAccount(userId);

            _userRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            _accRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(account);

            Func<Task> act = async () =>
                await _service.CreateUserAccountAsync(userId, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*already has an account*");
        }
    }
}
