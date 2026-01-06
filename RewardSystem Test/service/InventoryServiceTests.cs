using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.service
{
    public class InventoryServiceTests
    {
        private readonly Mock<IProductInventoryRepository> _invRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private InventoryService CreateSut() => new(_invRepo.Object, _uow.Object);

        [Fact]
        public async Task GetByProductIdAsync_EmptyId_ReturnsNull()
        {
            var sut = CreateSut();

            var result = await sut.GetByProductIdAsync(Guid.Empty);

            result.Should().BeNull();
        }

        [Fact]
        public async Task IncreaseStockAsync_InvalidQuantity_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.IncreaseStockAsync(Guid.NewGuid(), 0);

            await act.Should().ThrowAsync<System.ComponentModel.DataAnnotations.ValidationException>();
        }

        [Fact]
        public async Task IncreaseStockAsync_Valid_UpdatesAndSaves()
        {
            var sut = CreateSut();
            var pid = Guid.NewGuid();
            var inv = new ProductInventory(pid, 5);

            _invRepo.Setup(r => r.GetByProductIdAsync(pid, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(inv);

            await sut.IncreaseStockAsync(pid, 3);

            inv.StockQuantity.Should().Be(8);
            _invRepo.Verify(r => r.UpdateAsync(inv, It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
