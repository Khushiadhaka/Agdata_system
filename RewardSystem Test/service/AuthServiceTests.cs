using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Security;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Rewardsystem_Domain.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IUserAccountRepository> _accountRepo = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<IJwtTokenGenerator> _jwt = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private AuthService CreateSut() =>
            new(_userRepo.Object, _accountRepo.Object, _hasher.Object, _jwt.Object, _uow.Object);

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ThrowsInvalidCredentials()
        {
            var sut = CreateSut();
            _userRepo.Setup(r => r.GetByEmailAsync("user@mail.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            Func<Task> act = () => sut.LoginAsync("user@mail.com", "pwd");

            await act.Should().ThrowAsync<InvalidCredentialsException>();
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordCorrect_ReturnsToken()
        {
            var sut = CreateSut();
            var user = new User("Test", "user@mail.com", "E1", UserRole.Admin);
            var acc = new UserAccount(user.Id);
            acc.SetPasswordHash("HASH");

            _userRepo.Setup(r => r.GetByEmailAsync("user@mail.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);
            _accountRepo.Setup(r => r.GetByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(acc);
            _hasher.Setup(h => h.Verify("HASH", "pwd")).Returns(true);
            _jwt.Setup(j => j.GenerateToken(
                            user.Id,
                            user.Email.ToString(),
                            It.IsAny<IDictionary<string, string>>()))
                .Returns("JWT_TOKEN");

            var token = await sut.LoginAsync("user@mail.com", "pwd");

            token.Should().Be("JWT_TOKEN");
        }

        [Fact]
        public async Task RegisterAsync_ValidInput_CreatesUserAndAccount()
        {
            var sut = CreateSut();
            _userRepo.Setup(r => r.ExistsByEmailAsync("user@mail.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);
            _userRepo.Setup(r => r.ExistsByEmployeeIdAsync("E1", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);
            _hasher.Setup(h => h.Hash("pwd")).Returns("HASH");
            _jwt.Setup(j => j.GenerateToken(
                            It.IsAny<Guid>(),
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, string>>()))
                .Returns("TOKEN");

            var result = await sut.RegisterAsync("Name", "user@mail.com", "E1", "pwd");

            result.Should().Be("TOKEN");
            _userRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            _accountRepo.Verify(r => r.AddAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
