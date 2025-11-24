using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryRedemptionRecordRepository : IRedemptionRecordRepository
    {
        private readonly List<RedemptionRecord> _items = new();

        public Task AddAsync(RedemptionRecord entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(r => r.Id == id));
        }

        public Task<IReadOnlyList<RedemptionRecord>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<RedemptionRecord>)_items.ToList());
        }

        public void Update(RedemptionRecord entity)
        {
            var idx = _items.FindIndex(r => r.Id == entity.Id);
            if (idx >= 0)
                _items[idx] = entity;
        }

        public void Remove(RedemptionRecord entity)
        {
            _items.RemoveAll(r => r.Id == entity.Id);
        }

        public Task<IReadOnlyList<RedemptionRecord>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var list = _items.Where(r => r.UserId == userId).ToList();
            return Task.FromResult((IReadOnlyList<RedemptionRecord>)list);
        }

        public Task<IReadOnlyList<RedemptionRecord>> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            var list = _items.Where(r => r.ProductId == productId).ToList();
            return Task.FromResult((IReadOnlyList<RedemptionRecord>)list);
        }
    }

}
