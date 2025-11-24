using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Services.Interfaces
{
    // Manages events and event definitions
    public interface IEventService
    {
        Task<EventDefinition> CreateEventDefinitionAsync(
            string name,
            string description,
            int rewardPoints,
            CancellationToken cancellationToken = default);

        Task<EventDefinition?> GetEventDefinitionByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EventDefinition>> GetAllEventDefinitionsAsync(
            CancellationToken cancellationToken = default);

        Task<EventInstance> ScheduleEventInstanceAsync(
            Guid eventDefinitionId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default);

        Task AssignWinnerAsync(
            Guid eventInstanceId,
            Guid winnerUserId,
            int rank,
            CancellationToken cancellationToken = default);
    }
}
