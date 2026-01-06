using FluentAssertions;
using Moq;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RewardSystem_Test.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IProductInventoryRepository> _invRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private ProductService CreateSut() =>
            new(_productRepo.Object, _invRepo.Object, _uow.Object);

        [Fact]
        public async Task CreateProductAsync_InvalidRequiredPoints_Throws()
        {
            var sut = CreateSut();

            Func<Task> act = () => sut.CreateProductAsync("Name", null, 0, 1, null);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*RequiredPoints must be > 0*");
        }

        [Fact]
        public async Task CreateProductAsync_Valid_AddsProductAndInventory()
        {
            var sut = CreateSut();

            var product = await sut.CreateProductAsync("Name", "Desc", 100, 10, "SKU1");

            product.Name.Should().Be("Name");
            _productRepo.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _invRepo.Verify(r => r.AddAsync(It.IsAny<ProductInventory>(), It.IsAny<CancellationToken>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeactivateAsync_HasPendingRedemptions_Throws()
        {
            var sut = CreateSut();
            var pid = Guid.NewGuid();
            var product = new Product("P", null, 10);
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(product, pid);

            _productRepo.Setup(r => r.GetByIdAsync(pid, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);
            _productRepo.Setup(r => r.HasPendingRedemptionsAsync(pid, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            Func<Task> act = () => sut.DeactivateAsync(pid);

            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*pending redemptions*");
        }
    }
}

