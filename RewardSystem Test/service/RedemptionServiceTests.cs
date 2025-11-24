using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Implementations;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

public class RedemptionServiceTests
{
    private readonly Mock<IRedemptionRequestRepository> _requestRepo = new();
    private readonly Mock<IRedemptionRecordRepository> _recordRepo = new();
    private readonly Mock<IRedemptionProcessRepository> _processRepo = new();
    private readonly Mock<IUserAccountRepository> _accountRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IProductInventoryRepository> _inventoryRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private RedemptionService CreateSut()
    {
        return new RedemptionService(
            _requestRepo.Object,
            _recordRepo.Object,
            _processRepo.Object,
            _accountRepo.Object,
            _productRepo.Object,
            _inventoryRepo.Object,
            _uow.Object);
    }


    // -------------------------------------------------
    // CREATE REQUEST
    // -------------------------------------------------

    [Fact]
    public async Task CreateRedemptionRequestAsync_Should_Create_Request_When_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var account = new UserAccount(userId);
        account.SetPoints(1000);

        var product = new Rewardsystem_Domain.Domain.Entities.Product.Product("P", "", 200);

        var inv = new Rewardsystem_Domain.Domain.Entities.Product.ProductInventory(productId, 10);

        _accountRepo.Setup(x => x.GetByUserIdAsync(userId, default)).ReturnsAsync(account);
        _productRepo.Setup(x => x.GetByIdAsync(productId, default)).ReturnsAsync(product);
        _inventoryRepo.Setup(x => x.GetByProductIdAsync(productId, default)).ReturnsAsync(inv);

        var sut = CreateSut();

        // Act
        var request = await sut.CreateRedemptionRequestAsync(userId, productId, 200);

        // Assert
        request.UserId.Should().Be(userId);
        request.Status.Should().Be(RedemptionStatus.Pending);

        _requestRepo.Verify(x => x.AddAsync(It.IsAny<RedemptionRequest>(), default), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
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

        await act.Should().ThrowAsync<BusinessRuleException>();
            
    }




    // -------------------------------------------------
    // APPROVE
    // -------------------------------------------------

    [Fact]
    public async Task ApproveRedemptionAsync_Should_Approve_And_Deduct_Points()
    {
        var requestId = Guid.NewGuid();
        var request = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 200);

        var account = new UserAccount(request.UserId);
        account.SetPoints(500);

        _requestRepo.Setup(x => x.GetByIdAsync(requestId, default)).ReturnsAsync(request);
        _accountRepo.Setup(x => x.GetByUserIdAsync(request.UserId, default)).ReturnsAsync(account);

        var sut = CreateSut();

        await sut.ApproveRedemptionAsync(requestId);

        request.Status.Should().Be(RedemptionStatus.Approved);
        account.Points.Should().Be(300);

        _requestRepo.Verify(x => x.Update(request), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }


    // -------------------------------------------------
    // REJECT
    // -------------------------------------------------

    [Fact]
    public async Task RejectRedemptionAsync_Should_Reject()
    {
        var requestId = Guid.NewGuid();
        var request = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 200);

        _requestRepo.Setup(x => x.GetByIdAsync(requestId, default)).ReturnsAsync(request);

        var sut = CreateSut();

        await sut.RejectRedemptionAsync(requestId);

        request.Status.Should().Be(RedemptionStatus.Rejected);
    }



    // -------------------------------------------------
    // COMPLETE
    // -------------------------------------------------

    [Fact]
    public async Task CompleteRedemptionAsync_Should_Mark_And_Create_Record()
    {
        var reqId = Guid.NewGuid();
        var req = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 200);
        req.Approve();

        var inv = new Rewardsystem_Domain.Domain.Entities.Product.ProductInventory(req.ProductId, 10);

        _requestRepo.Setup(x => x.GetByIdAsync(reqId, default)).ReturnsAsync(req);
        _inventoryRepo.Setup(x => x.GetByProductIdAsync(req.ProductId, default)).ReturnsAsync(inv);

        var sut = CreateSut();

        await sut.CompleteRedemptionAsync(reqId);

        req.Status.Should().Be(RedemptionStatus.Completed);
        inv.StockQuantity.Should().Be(9);

        _recordRepo.Verify(x => x.AddAsync(It.IsAny<RedemptionRecord>(), default), Times.Once);
    }
}

