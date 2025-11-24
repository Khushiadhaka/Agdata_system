using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryTransactionRepository : ITransactionRepository
    {
        // IMPORTANT: use _items, not _entities
        private readonly List<Transaction> _items = new();

        public Task AddAsync(Transaction entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(t => t.Id == id));
        }

        public Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<Transaction>)_items.ToList());
        }

        public void Update(Transaction entity)
        {
            var idx = _items.FindIndex(t => t.Id == entity.Id);
            if (idx >= 0)
                _items[idx] = entity;
        }

        public void Remove(Transaction entity)
        {
            _items.RemoveAll(t => t.Id == entity.Id);
        }

        public Task<IReadOnlyList<Transaction>> GetByTypeAsync(
            TransactionType type,
            CancellationToken cancellationToken = default)
        {
            var list = _items.Where(t => t.Type == type).ToList();
            return Task.FromResult((IReadOnlyList<Transaction>)list);
        }
    }

}
