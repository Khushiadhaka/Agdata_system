using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryRedemptionProcessRepository : IRedemptionProcessRepository
    {
        private readonly List<RedemptionProcess> _items = new();

        public Task AddAsync(RedemptionProcess entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<RedemptionProcess?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(x => x.Id == id));
        }

        public Task<IReadOnlyList<RedemptionProcess>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<RedemptionProcess>)_items.ToList());
        }

        public void Update(RedemptionProcess entity)
        {
            var idx = _items.FindIndex(x => x.Id == entity.Id);
            if (idx != -1)
                _items[idx] = entity;
        }

        public void Remove(RedemptionProcess entity)
        {
            _items.RemoveAll(x => x.Id == entity.Id);
        }
    }

}
