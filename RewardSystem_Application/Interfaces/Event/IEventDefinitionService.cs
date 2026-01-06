using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Event
{
    // Manage event definition templates
    public interface IEventDefinitionService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Event.EventDefinition> CreateAsync(
            string name,
            string? description,
            int rewardPoints,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Event.EventDefinition> UpdateAsync(
            Guid id,
            string name,
            string? description,
            int rewardPoints,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Event.EventDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Event.EventDefinition>> ListAsync(bool includeInactive = false, CancellationToken ct = default);

        Task DeactivateAsync(Guid id, CancellationToken ct = default);
    }
}
