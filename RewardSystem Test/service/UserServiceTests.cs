using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IUserAccountRepository> _accRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private UserService CreateSut() =>
            new(_userRepo.Object, _accRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateUserAsync_DuplicateEmail_ThrowsDuplicateUser()
        {
            var sut = CreateSut();
            _userRepo.Setup(r => r.ExistsByEmailAsync("user@mail.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

            Func<Task> act = () => sut.CreateUserAsync("Name", "user@mail.com", "E1", "User");

            await act.Should().ThrowAsync<DuplicateUserException>()
                .WithMessage("*Email exists*");
        }

        [Fact]
        public async Task DeleteUserAsync_AlreadyDeleted_ReturnsFalse()
        {
            var sut = CreateSut();
            var user = new User("N", "u@mail.com", "E1", UserRole.User);
            typeof(User).GetProperty(nameof(User.IsDeleted))!.SetValue(user, true);

            _userRepo.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(user);

            var result = await sut.DeleteUserAsync(user.Id);

            result.Should().BeFalse();
        }
    }
}
