using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryRewardRepository : IRewardRepository
    {
        private readonly List<Reward> _items = new();

        public Task AddAsync(Reward entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<Reward?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(x => x.Id == id));
        }

        public Task<IReadOnlyList<Reward>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<Reward>)_items.ToList());
        }

        public void Update(Reward entity)
        {
            var idx = _items.FindIndex(x => x.Id == entity.Id);
            if (idx != -1) _items[idx] = entity;
        }

        public void Remove(Reward entity)
        {
            _items.RemoveAll(x => x.Id == entity.Id);
        }
    }

}
