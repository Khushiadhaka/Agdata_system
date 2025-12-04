using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class UserProfileServiceTests
    {
        private readonly Mock<IUserProfileRepository> _profileRepo = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private UserProfileService CreateSut() =>
            new(_profileRepo.Object, _userRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateOrUpdateAsync_UserNotFound_Throws()
        {
            var sut = CreateSut();
            _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((User?)null);

            Func<Task> act = () => sut.CreateOrUpdateAsync(Guid.NewGuid(), "p", "d", "l");

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*User not found*");
        }
    }
}
