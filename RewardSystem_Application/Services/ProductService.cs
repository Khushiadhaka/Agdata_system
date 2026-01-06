// Product service: manage products and coordinate inventory creation/updates.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Product;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Application.Services
{
    // Product service: manage products and coordinate inventory creation/updates.
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductInventoryRepository _inventoryRepo;
        private readonly IUnitOfWork _uow;

        public ProductService(
            IProductRepository productRepo,
            IProductInventoryRepository inventoryRepo,
            IUnitOfWork uow)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create product and its initial inventory.
        public async Task<Product> CreateProductAsync(
            string name,
            string? description,
            int requiredPoints,
            int initialStock,
            string? sku,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ValidationException("Name required.");
            if (requiredPoints <= 0) throw new ValidationException("RequiredPoints must be > 0.");
            if (initialStock < 0) throw new ValidationException("InitialStock cannot be negative.");

            var product = new Product(name.Trim(), description?.Trim(), requiredPoints);

            // Set SKU if provided (domain Product now supports SKU via SetSKU).
            if (!string.IsNullOrWhiteSpace(sku))
            {
                product.SetSKU(sku.Trim());
            }

            await _productRepo.AddAsync(product, ct);

            var inventory = new ProductInventory(product.Id, initialStock);
            await _inventoryRepo.AddAsync(inventory, ct);

            await _uow.SaveChangesAsync(ct);
            return product;
        }

        // Get product by id (or null if not found / invalid id).
        public async Task<Product?> GetByIdAsync(Guid productId, CancellationToken ct = default)
        {
            if (productId == Guid.Empty) return null;
            return await _productRepo.GetByIdAsync(productId, ct);
        }

        // List products, optionally excluding inactive ones.
        public async Task<IReadOnlyList<Product>> ListAsync(
            bool includeInactive = false,
            CancellationToken ct = default)
        {
            var list = await _productRepo.GetAllAsync(ct);
            if (!includeInactive)
            {
                list = list.Where(p => p.IsActive).ToList();
            }

            return list;
        }

        // Update basic product information.
        public async Task<Product> UpdateProductAsync(
            Guid productId,
            string name,
            string? description,
            int requiredPoints,
            string? sku,
            CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(productId, ct)
                          ?? throw new InvalidOperationException("Product not found.");

            product.Update(name.Trim(), description?.Trim(), requiredPoints);

            if (!string.IsNullOrWhiteSpace(sku))
            {
                product.SetSKU(sku.Trim());
            }

            await _productRepo.UpdateAsync(product, ct);
            await _uow.SaveChangesAsync(ct);
            return product;
        }

        // Adjust stock for a product (positive = increase, negative = decrease).
        public async Task AdjustStockAsync(Guid productId, int delta, CancellationToken ct = default)
        {
            var inventory = await _inventoryRepo.GetByProductIdAsync(productId, ct)
                            ?? throw new InvalidOperationException("Inventory not found.");

            if (delta > 0)
            {
                inventory.IncreaseStock(delta);
            }
            else if (delta < 0)
            {
                inventory.ReduceStock(Math.Abs(delta));
            }

            await _inventoryRepo.UpdateAsync(inventory, ct);
            await _uow.SaveChangesAsync(ct);
        }

        // Deactivate product ensuring there are no pending redemptions.
        public async Task DeactivateAsync(Guid productId, CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(productId, ct)
                          ?? throw new InvalidOperationException("Product not found.");

            var hasPending = await _productRepo.HasPendingRedemptionsAsync(productId, ct);
            if (hasPending)
            {
                throw new BusinessRuleException("Cannot deactivate product with pending redemptions.");
            }

            product.Deactivate();
            await _productRepo.UpdateAsync(product, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
