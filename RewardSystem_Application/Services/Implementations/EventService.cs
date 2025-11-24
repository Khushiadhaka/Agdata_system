using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Services.Implementations
{
    // Application service for event definitions and instances
    public sealed class EventService : IEventService
    {
        private readonly IEventDefinitionRepository _definitionRepository;
        private readonly IEventInstanceRepository _instanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventService(
            IEventDefinitionRepository definitionRepository,
            IEventInstanceRepository instanceRepository,
            IUnitOfWork unitOfWork)
        {
            _definitionRepository = definitionRepository ?? throw new ArgumentNullException(nameof(definitionRepository));
            _instanceRepository = instanceRepository ?? throw new ArgumentNullException(nameof(instanceRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Create event definition
        public async Task<EventDefinition> CreateEventDefinitionAsync(
            string name,
            string description,
            int rewardPoints,
            CancellationToken cancellationToken = default)
        {
            var definition = new EventDefinition(name, description, rewardPoints);

            await _definitionRepository.AddAsync(definition, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return definition;
        }

        // Get definition by id
        public Task<EventDefinition?> GetEventDefinitionByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("EventDefinitionId cannot be empty.");

            return _definitionRepository.GetByIdAsync(id, cancellationToken);
        }

        // Get all definitions
        public Task<IReadOnlyList<EventDefinition>> GetAllEventDefinitionsAsync(
            CancellationToken cancellationToken = default)
        {
            return _definitionRepository.GetAllAsync(cancellationToken);
        }

        // Schedule instance
        public async Task<EventInstance> ScheduleEventInstanceAsync(
            Guid eventDefinitionId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default)
        {
            if (eventDefinitionId == Guid.Empty)
                throw new ValidationException("EventDefinitionId cannot be empty.");

            var def = await _definitionRepository.GetByIdAsync(eventDefinitionId, cancellationToken);
            if (def == null)
                throw new BusinessRuleException("Event definition not found.");

            var instance = new EventInstance(eventDefinitionId, startTime, endTime);

            await _instanceRepository.AddAsync(instance, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return instance;
        }

        // Assign winner to event instance
        public async Task AssignWinnerAsync(
            Guid eventInstanceId,
            Guid winnerUserId,
            int rank,
            CancellationToken cancellationToken = default)
        {
            if (eventInstanceId == Guid.Empty)
                throw new ValidationException("EventInstanceId cannot be empty.");

            var instance = await _instanceRepository.GetByIdAsync(eventInstanceId, cancellationToken);
            if (instance == null)
                throw new BusinessRuleException("Event instance not found.");

            instance.AssignWinner(winnerUserId, rank);

            _instanceRepository.Update(instance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
