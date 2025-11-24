using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryRedemptionRequestRepository : IRedemptionRequestRepository
    {
        private readonly List<RedemptionRequest> _items = new();

        public Task AddAsync(RedemptionRequest entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<RedemptionRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(r => r.Id == id));
        }

        public Task<IReadOnlyList<RedemptionRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<RedemptionRequest>)_items.ToList());
        }

        public void Update(RedemptionRequest entity)
        {
            var idx = _items.FindIndex(r => r.Id == entity.Id);
            if (idx >= 0)
                _items[idx] = entity;
        }

        public void Remove(RedemptionRequest entity)
        {
            _items.RemoveAll(r => r.Id == entity.Id);
        }

        public Task<IReadOnlyList<RedemptionRequest>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var list = _items.Where(r => r.UserId == userId).ToList();
            return Task.FromResult((IReadOnlyList<RedemptionRequest>)list);
        }

        public Task<IReadOnlyList<RedemptionRequest>> GetByStatusAsync(
            RedemptionStatus status,
            CancellationToken cancellationToken = default)
        {
            var list = _items.Where(r => r.Status == status).ToList();
            return Task.FromResult((IReadOnlyList<RedemptionRequest>)list);
        }
    }

}
