using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class RedemptionRequestServiceTests
    {
        private readonly Mock<IRedemptionRequestRepository> _reqRepo = new();
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IProductInventoryRepository> _invRepo = new();
        private readonly Mock<IUserAccountRepository> _accountRepo = new();
        private readonly Mock<IRedemptionProcessRepository> _processRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RedemptionRequestService CreateSut() =>
            new(_reqRepo.Object, _productRepo.Object, _invRepo.Object,
                _accountRepo.Object, _processRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateAsync_InvalidPoints_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), 0);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*PointsUsed must be positive*");
        }

        [Fact]
        public async Task CreateAsync_InsufficientPoints_ThrowsInsufficientPoints()
        {
            var sut = CreateSut();
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product("P", null, 10);
            var acc = new UserAccount(userId);
            acc.SetPoints(10);

            _productRepo.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);
            _invRepo.Setup(r => r.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ProductInventory?)null);
            _accountRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(acc);

            Func<Task> act = () => sut.CreateAsync(userId, productId, 50);

            await act.Should().ThrowAsync<InsufficientPointsException>();
        }

        [Fact]
        public async Task CreateAsync_Valid_CreatesRequest()
        {
            var sut = CreateSut();
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product("P", null, 10);
            var acc = new UserAccount(userId);
            acc.SetPoints(100);

            _productRepo.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);
            _invRepo.Setup(r => r.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ProductInventory?)null);
            _accountRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(acc);
            _reqRepo.Setup(r => r.GetPendingByUserAndProductAsync(userId, productId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((RedemptionRequest?)null);

            var req = await sut.CreateAsync(userId, productId, 50);

            req.UserId.Should().Be(userId);
            req.ProductId.Should().Be(productId);
            _reqRepo.Verify(r => r.AddAsync(It.IsAny<RedemptionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
