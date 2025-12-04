using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class UserAccountServiceTests
    {
        private readonly Mock<IUserAccountRepository> _accRepo = new();
        private readonly Mock<IPointsTransactionRepository> _pointsRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private UserAccountService CreateSut() =>
            new(_accRepo.Object, _pointsRepo.Object, _uow.Object);

        [Fact]
        public async Task GetBalanceAsync_EmptyUserId_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.GetBalanceAsync(Guid.Empty);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task AddPointsAsync_InactiveAccount_ThrowsBusinessRule()
        {
            var sut = CreateSut();
            var uid = Guid.NewGuid();
            var acc = new UserAccount(uid);
            typeof(UserAccount).GetProperty(nameof(UserAccount.Status))!.SetValue(acc, AccountStatus.Inactive);

            _accRepo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(acc);

            Func<Task> act = () => sut.AddPointsAsync(uid, 10);

            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*active accounts*");
        }
    }
}
