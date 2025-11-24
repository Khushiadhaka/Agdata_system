using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Implementations;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Services
{
    public sealed class RedemptionServiceTests
    {
        private readonly Mock<IRedemptionRequestRepository> _reqRepo = new();
        private readonly Mock<IRedemptionRecordRepository> _recRepo = new();
        private readonly Mock<IRedemptionProcessRepository> _procRepo = new();
        private readonly Mock<IUserAccountRepository> _accountRepo = new();
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IProductInventoryRepository> _inventoryRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RedemptionService CreateSut()
        {
            return new RedemptionService(
                _reqRepo.Object,
                _recRepo.Object,
                _procRepo.Object,
                _accountRepo.Object,
                _productRepo.Object,
                _inventoryRepo.Object,
                _uow.Object
            );
        }

        [Fact]
        public async Task CreateRedemptionRequest_Should_Create_When_Valid()
        {
            var user = Guid.NewGuid();
            var product = Guid.NewGuid();

            var acc = new UserAccount(user);
            acc.SetPoints(500);

            var prod = new Product("Phone", "", 200);

            var inv = new ProductInventory(product, 3);

            _accountRepo.Setup(x => x.GetByUserIdAsync(user, default)).ReturnsAsync(acc);
            _productRepo.Setup(x => x.GetByIdAsync(product, default)).ReturnsAsync(prod);
            _inventoryRepo.Setup(x => x.GetByProductIdAsync(product, default)).ReturnsAsync(inv);

            var sut = CreateSut();

            var result = await sut.CreateRedemptionRequestAsync(user, product, 300);

            result.Should().NotBeNull();
            result.Status.Should().Be(RedemptionStatus.Pending);

            _reqRepo.Verify(x => x.AddAsync(It.IsAny<RedemptionRequest>(), default), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateRedemptionRequest_Should_Throw_When_User_Not_Found()
        {
            var sut = CreateSut();

            _accountRepo.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((UserAccount)null);

            Func<Task> act = () => sut.CreateRedemptionRequestAsync(Guid.NewGuid(), Guid.NewGuid(), 100);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task CreateRedemptionRequest_Should_Throw_When_Product_Not_Found()
        {
            var uid = Guid.NewGuid();

            _accountRepo.Setup(x => x.GetByUserIdAsync(uid, default))
                .ReturnsAsync(new UserAccount(uid));

            _productRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((Product)null);

            var sut = CreateSut();

            Func<Task> act = () => sut.CreateRedemptionRequestAsync(uid, Guid.NewGuid(), 100);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task CreateRedemptionRequestAsync_Should_Throw_When_User_Has_Insufficient_Points()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var account = new UserAccount(userId);
            account.SetPoints(20);

            var product = new Rewardsystem_Domain.Domain.Entities.Product.Product("P", "", 200);
            var inv = new Rewardsystem_Domain.Domain.Entities.Product.ProductInventory(productId, 5);

            _accountRepo.Setup(x => x.GetByUserIdAsync(userId, default)).ReturnsAsync(account);
            _productRepo.Setup(x => x.GetByIdAsync(productId, default)).ReturnsAsync(product);
            _inventoryRepo.Setup(x => x.GetByProductIdAsync(productId, default)).ReturnsAsync(inv);

            var sut = CreateSut();

            Func<Task> act = () => sut.CreateRedemptionRequestAsync(userId, productId, 200);

            //sirf exception type check kar rahe hain, message nahi
            await act.Should().ThrowAsync<BusinessRuleException>();
        }


        [Fact]
        public async Task ApproveRedemption_Should_Deduct_Points()
        {
            var uid = Guid.NewGuid();
            var pid = Guid.NewGuid();

            var req = new RedemptionRequest(uid, pid, 200);
            var acc = new UserAccount(uid);
            acc.SetPoints(300);

            _reqRepo.Setup(x => x.GetByIdAsync(req.Id, default)).ReturnsAsync(req);
            _accountRepo.Setup(x => x.GetByUserIdAsync(uid, default)).ReturnsAsync(acc);

            var sut = CreateSut();

            await sut.ApproveRedemptionAsync(req.Id);

            req.Status.Should().Be(RedemptionStatus.Approved);
            acc.Points.Should().Be(100);

            _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task RejectRedemption_Should_Mark_Rejected()
        {
            var uid = Guid.NewGuid();
            var pid = Guid.NewGuid();

            var req = new RedemptionRequest(uid, pid, 200);

            _reqRepo.Setup(x => x.GetByIdAsync(req.Id, default)).ReturnsAsync(req);

            var sut = CreateSut();

            await sut.RejectRedemptionAsync(req.Id);

            req.Status.Should().Be(RedemptionStatus.Rejected);

            _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CompleteRedemption_Should_Add_Record_And_Reduce_Stock()
        {
            var uid = Guid.NewGuid();
            var pid = Guid.NewGuid();

            var req = new RedemptionRequest(uid, pid, 200);
            req.Approve();

            var inv = new ProductInventory(pid, 3);

            _reqRepo.Setup(x => x.GetByIdAsync(req.Id, default)).ReturnsAsync(req);
            _inventoryRepo.Setup(x => x.GetByProductIdAsync(pid, default)).ReturnsAsync(inv);

            var sut = CreateSut();

            await sut.CompleteRedemptionAsync(req.Id);

            inv.StockQuantity.Should().Be(2);
            req.Status.Should().Be(RedemptionStatus.Completed);

            _recRepo.Verify(x => x.AddAsync(It.IsAny<RedemptionRecord>(), default), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}
