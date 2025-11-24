using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _items = new();

        public Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(p => p.Id == id));
        }

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<Product>)_items.ToList());
        }

        public void Update(Product entity)
        {
            var idx = _items.FindIndex(p => p.Id == entity.Id);
            if (idx >= 0)
                _items[idx] = entity;
        }

        public void Remove(Product entity)
        {
            _items.RemoveAll(p => p.Id == entity.Id);
        }

        public Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            var active = _items.Where(p => p.IsActive).ToList();
            return Task.FromResult((IReadOnlyList<Product>)active);
        }
    }

}
