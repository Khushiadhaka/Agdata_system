using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation for ProductInventory repository
    public sealed class InMemoryProductInventoryRepository : IProductInventoryRepository
    {
        // Simple in-memory store
        private readonly List<ProductInventory> _items = new();

        // Add a new inventory record
        public Task AddAsync(ProductInventory entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _items.Add(entity);
            return Task.CompletedTask;
        }

        // Get by entity Id
        public Task<ProductInventory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item);
        }

        // Get all records
        public Task<IReadOnlyList<ProductInventory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<ProductInventory> result = _items.ToList();
            return Task.FromResult(result);
        }

        // Update existing record
        public void Update(ProductInventory entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            var index = _items.FindIndex(x => x.Id == entity.Id);
            if (index >= 0)
            {
                _items[index] = entity;
            }
        }

        // Remove record
        public void Remove(ProductInventory entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _items.RemoveAll(x => x.Id == entity.Id);
        }

        // Get inventory by ProductId
        public Task<ProductInventory?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);
            return Task.FromResult(item);
        }

        // Get only active inventory records (extra helper)
        public Task<IReadOnlyList<ProductInventory>> GetActiveAsync(
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<ProductInventory> active = _items
                .Where(x => x.IsActive)
                .ToList();

            return Task.FromResult(active);
        }
    }
}
