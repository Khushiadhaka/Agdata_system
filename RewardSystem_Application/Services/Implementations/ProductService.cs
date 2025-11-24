using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Implementations
{
    // Application service for product and inventory operations
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(
            IProductRepository productRepository,
            IProductInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Creates a new product
        public async Task<Product> CreateProductAsync(
            string name,
            string description,
            int requiredPoints,
            CancellationToken cancellationToken = default)
        {
            var product = new Product(name, description, requiredPoints);

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product;
        }

        // Gets product by id
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Product id cannot be empty.");

            return _productRepository.GetByIdAsync(id, cancellationToken);
        }

        // Gets all products
        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _productRepository.GetAllAsync(cancellationToken);
        }

        // Updates product
        public async Task UpdateProductAsync(
            Guid id,
            string name,
            string description,
            int requiredPoints,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Product id cannot be empty.");

            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (product == null)
                throw new BusinessRuleException("Product not found.");

            product.Update(name, description, requiredPoints);

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Deactivates product
        public async Task DeactivateProductAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Product id cannot be empty.");

            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (product == null)
                throw new BusinessRuleException("Product not found.");

            product.Deactivate();
            _productRepository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Set initial inventory
        public async Task<ProductInventory> SetInitialInventoryAsync(
            Guid productId,
            int stockQuantity,
            CancellationToken cancellationToken = default)
        {
            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
                throw new BusinessRuleException("Product not found.");

            var existing = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
            if (existing != null)
                throw new BusinessRuleException("Inventory already exists for this product.");

            var inventory = new ProductInventory(productId, stockQuantity);

            await _inventoryRepository.AddAsync(inventory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return inventory;
        }

        // Increase stock
        public async Task IncreaseStockAsync(
            Guid productId,
            int quantity,
            CancellationToken cancellationToken = default)
        {
            var inventory = await LoadInventory(productId, cancellationToken);
            inventory.IncreaseStock(quantity);

            _inventoryRepository.Update(inventory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Reduce stock
        public async Task ReduceStockAsync(
            Guid productId,
            int quantity,
            CancellationToken cancellationToken = default)
        {
            var inventory = await LoadInventory(productId, cancellationToken);
            inventory.ReduceStock(quantity);

            _inventoryRepository.Update(inventory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Loads inventory or throws
        private async Task<ProductInventory> LoadInventory(Guid productId, CancellationToken cancellationToken)
        {
            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            var inventory = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
            if (inventory == null)
                throw new BusinessRuleException("Inventory record not found.");

            return inventory;
        }
    }
}
