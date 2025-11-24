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
using Xunit;

namespace RewardSystem_Test.Services
{
    public sealed class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepo;
        private readonly Mock<IProductInventoryRepository> _inventoryRepo;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _productRepo = new Mock<IProductRepository>();
            _inventoryRepo = new Mock<IProductInventoryRepository>();
            _uow = new Mock<IUnitOfWork>();

            // IUnitOfWork.SaveChangesAsync -> Task<int>
            _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _service = new ProductService(
                _productRepo.Object,
                _inventoryRepo.Object,
                _uow.Object);
        }

        // ---------------------------------------------------------
        // CreateProductAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task CreateProductAsync_Should_Create_And_Save_Product()
        {
            // Act
            var product = await _service.CreateProductAsync(
                "Pen",
                "Blue ball pen",
                50,
                CancellationToken.None);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be("Pen");
            product.RequiredPoints.Should().Be(50);
            product.IsActive.Should().BeTrue();

            _productRepo.Verify(
                r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uow.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // GetByIdAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.GetByIdAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Product id cannot be empty*");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Product_From_Repository()
        {
            var id = Guid.NewGuid();
            var product = new Product("Pen", "Desc", 10);

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            var result = await _service.GetByIdAsync(id, CancellationToken.None);

            result.Should().Be(product);

            _productRepo.Verify(
                r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // GetAllAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Products()
        {
            var list = new[]
            {
                new Product("P1", "D1", 10),
                new Product("P2", "D2", 20)
            };

            _productRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(list);

            var result = await _service.GetAllAsync(CancellationToken.None);

            result.Should().HaveCount(2);
            _productRepo.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // UpdateProductAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task UpdateProductAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.UpdateProductAsync(
                    Guid.Empty,
                    "New",
                    "New desc",
                    100,
                    CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Product id cannot be empty*");
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Throw_When_Product_Not_Found()
        {
            _productRepo.Setup(r => r.GetByIdAsync(
                                    It.IsAny<Guid>(),
                                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Product?)null);

            Func<Task> act = async () =>
                await _service.UpdateProductAsync(
                    Guid.NewGuid(),
                    "New",
                    "New desc",
                    100,
                    CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Product not found*");
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Update_And_Save()
        {
            var id = Guid.NewGuid();
            var product = new Product("Old", "Old desc", 50);

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            await _service.UpdateProductAsync(
                id,
                "New",
                "New desc",
                100,
                CancellationToken.None);

            product.Name.Should().Be("New");
            product.Description.Should().Be("New desc");
            product.RequiredPoints.Should().Be(100);

            _productRepo.Verify(r => r.Update(product), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // DeactivateProductAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task DeactivateProductAsync_Should_Throw_When_Id_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.DeactivateProductAsync(Guid.Empty, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*Product id cannot be empty*");
        }

        [Fact]
        public async Task DeactivateProductAsync_Should_Throw_When_Product_Not_Found()
        {
            _productRepo.Setup(r => r.GetByIdAsync(
                                    It.IsAny<Guid>(),
                                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Product?)null);

            Func<Task> act = async () =>
                await _service.DeactivateProductAsync(Guid.NewGuid(), CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Product not found*");
        }

        [Fact]
        public async Task DeactivateProductAsync_Should_Deactivate_And_Save()
        {
            var id = Guid.NewGuid();
            var product = new Product("P", "D", 10);

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            await _service.DeactivateProductAsync(id, CancellationToken.None);

            product.IsActive.Should().BeFalse();

            _productRepo.Verify(r => r.Update(product), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // SetInitialInventoryAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task SetInitialInventoryAsync_Should_Throw_When_ProductId_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.SetInitialInventoryAsync(
                    Guid.Empty,
                    10,
                    CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*ProductId cannot be empty*");
        }

        [Fact]
        public async Task SetInitialInventoryAsync_Should_Throw_When_Product_Not_Found()
        {
            var id = Guid.NewGuid();

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Product?)null);

            Func<Task> act = async () =>
                await _service.SetInitialInventoryAsync(id, 10, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Product not found*");
        }

        [Fact]
        public async Task SetInitialInventoryAsync_Should_Throw_When_Inventory_Already_Exists()
        {
            var id = Guid.NewGuid();
            var product = new Product("P", "D", 10);
            var existingInventory = new ProductInventory(id, 5);

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingInventory);

            Func<Task> act = async () =>
                await _service.SetInitialInventoryAsync(id, 10, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Inventory already exists*");
        }

        [Fact]
        public async Task SetInitialInventoryAsync_Should_Create_And_Save()
        {
            var id = Guid.NewGuid();
            var product = new Product("P", "D", 10);

            _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((ProductInventory?)null);

            var inventory = await _service.SetInitialInventoryAsync(id, 20, CancellationToken.None);

            inventory.ProductId.Should().Be(id);
            inventory.StockQuantity.Should().Be(20);

            _inventoryRepo.Verify(
                r => r.AddAsync(It.IsAny<ProductInventory>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uow.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ---------------------------------------------------------
        // IncreaseStockAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task IncreaseStockAsync_Should_Throw_When_ProductId_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.IncreaseStockAsync(Guid.Empty, 5, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*ProductId cannot be empty*");
        }

        [Fact]
        public async Task IncreaseStockAsync_Should_Throw_When_Inventory_Not_Found()
        {
            var id = Guid.NewGuid();

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((ProductInventory?)null);

            Func<Task> act = async () =>
                await _service.IncreaseStockAsync(id, 5, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Inventory record not found*");
        }

        [Fact]
        public async Task IncreaseStockAsync_Should_Update_Stock_And_Save()
        {
            var id = Guid.NewGuid();
            var inventory = new ProductInventory(id, 10);

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(inventory);

            await _service.IncreaseStockAsync(id, 5, CancellationToken.None);

            inventory.StockQuantity.Should().Be(15);

            _inventoryRepo.Verify(r => r.Update(inventory), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------
        // ReduceStockAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task ReduceStockAsync_Should_Throw_When_ProductId_Is_Empty()
        {
            Func<Task> act = async () =>
                await _service.ReduceStockAsync(Guid.Empty, 5, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("*ProductId cannot be empty*");
        }

        [Fact]
        public async Task ReduceStockAsync_Should_Throw_When_Inventory_Not_Found()
        {
            var id = Guid.NewGuid();

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((ProductInventory?)null);

            Func<Task> act = async () =>
                await _service.ReduceStockAsync(id, 5, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessRuleException>()
                     .WithMessage("*Inventory record not found*");
        }

        [Fact]
        public async Task ReduceStockAsync_Should_Update_Stock_And_Save()
        {
            var id = Guid.NewGuid();
            var inventory = new ProductInventory(id, 20);

            _inventoryRepo.Setup(r => r.GetByProductIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(inventory);

            await _service.ReduceStockAsync(id, 5, CancellationToken.None);

            inventory.StockQuantity.Should().Be(15);

            _inventoryRepo.Verify(r => r.Update(inventory), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

