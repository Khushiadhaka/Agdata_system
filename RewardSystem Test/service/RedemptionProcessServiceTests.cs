using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class RedemptionProcessServiceTests
    {
        private readonly Mock<IRedemptionRequestRepository> _reqRepo = new();
        private readonly Mock<IRedemptionRecordRepository> _recRepo = new();
        private readonly Mock<IUserAccountRepository> _accRepo = new();
        private readonly Mock<IProductInventoryRepository> _invRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private RedemptionProcessService CreateSut() =>
            new(_reqRepo.Object, _recRepo.Object, _accRepo.Object, _invRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateRecordAsync_OutOfStock_Throws()
        {
            var sut = CreateSut();
            var productId = Guid.NewGuid();

            _invRepo.Setup(r => r.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ProductInventory(productId, 0));

            Func<Task> act = () => sut.CreateRecordAsync(Guid.NewGuid(), productId);

            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*Out of stock*");
        }

        [Fact]
        public async Task CompleteRequestAsync_NotApproved_Throws()
        {
            var sut = CreateSut();
            var req = new RedemptionRequest(Guid.NewGuid(), Guid.NewGuid(), 10);
            // default status Pending, not Approved

            _reqRepo.Setup(r => r.GetByIdAsync(req.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(req);

            Func<Task> act = () => sut.CompleteRequestAsync(req.Id);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }
    }
}
