using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.Controllers;
using RewardSystem_API.DTOs.Product;
using RewardSystem_API.Services;
using Xunit;

namespace RewardSystem_Test.Controllers
{
    public class ProductControllerTests
    {
        private readonly FakeProductApiService _service;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _service = new FakeProductApiService();
            _controller = new ProductController(_service);
        }

        // =============== GET ALL ===============

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithList()
        {
            // Arrange
            _service.Products.Add(new ProductDto { Id = Guid.NewGuid(), Name = "P1" });

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IReadOnlyList<ProductDto>>(ok.Value);
            Assert.Single(list);
        }

        // =============== GET BY ID ===============

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _service.Products.Add(new ProductDto { Id = id, Name = "Existing" });

            // Act
            var result = await _controller.GetById(id, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ProductDto>(ok.Value);
            Assert.Equal(id, dto.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenProductMissing()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.GetById(id, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // =============== CREATE ===============

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new ProductCreateDto();

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "New Product",
                Description = "Desc",
                RequiredPoints = 100,
                InitialStock = 5,
                SKU = "SKU-1"
            };

            // Act
            var result = await _controller.Create(dto, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var product = Assert.IsType<ProductDto>(created.Value);
            Assert.Equal("New Product", product.Name);
            Assert.Equal(nameof(ProductController.GetById), created.ActionName);
        }

        // =============== UPDATE ===============

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto();

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenProductMissing()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto
            {
                Name = "Updated",
                Description = "D",
                RequiredPoints = 100,
                SKU = "SKU-1"
            };

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _service.Products.Add(new ProductDto { Id = id, Name = "Old" });

            var dto = new ProductUpdateDto
            {
                Name = "Updated",
                Description = "New desc",
                RequiredPoints = 150,
                SKU = "SKU-2"
            };

            // Act
            var result = await _controller.Update(id, dto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsType<ProductDto>(ok.Value);
            Assert.Equal("Updated", updated.Name);
        }

        // =============== DELETE ===============

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _service.Products.Add(new ProductDto { Id = id, Name = "ToDelete" });

            // Act
            var result = await _controller.Delete(id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }

    
    public sealed class FakeProductApiService : IProductApiService
    {
        public List<ProductDto> Products { get; } = new();

        public Task<IReadOnlyList<ProductDto>> ListAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<ProductDto>>(Products);

        public Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var p = Products.Find(x => x.Id == id);
            return Task.FromResult<ProductDto?>(p);
        }

        public Task<ProductDto> CreateAsync(ProductCreateDto dto, CancellationToken cancellationToken = default)
        {
            var product = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                RequiredPoints = dto.RequiredPoints,
                SKU = dto.SKU
            };

            Products.Add(product);
            return Task.FromResult(product);
        }

        public Task<ProductDto?> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var product = Products.Find(x => x.Id == id);
            if (product == null)
                return Task.FromResult<ProductDto?>(null);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.RequiredPoints = dto.RequiredPoints;
            product.SKU = dto.SKU;

            return Task.FromResult<ProductDto?>(product);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Products.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }

        

        public Task<ProductInventoryDto?> GetInventoryAsync(Guid productId, CancellationToken cancellationToken = default)
            => Task.FromResult<ProductInventoryDto?>(null);

        public Task UpdateStockAsync(Guid productId, int quantityChange, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}

