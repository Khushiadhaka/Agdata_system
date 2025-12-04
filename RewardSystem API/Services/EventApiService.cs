using AutoMapper;
using RewardSystem_API.DTOs.Event;
using RewardSystem_Application.Interfaces.Event;


namespace RewardSystem_API.Services
{
    /// <summary>
    /// Contract used by EventController.
    /// </summary>
    public interface IEventApiService
    {
        // ---------- Event Definitions ----------

        Task<EventDefinitionDto?> GetDefinitionByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EventDefinitionDto>> ListDefinitionsAsync(
            CancellationToken cancellationToken = default);

        Task<EventDefinitionDto> CreateDefinitionAsync(
            EventDefinitionCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<EventDefinitionDto?> UpdateDefinitionAsync(
            Guid id,
            EventDefinitionUpdateDto dto,
            CancellationToken cancellationToken = default);

        // ---------- Event Instances ----------

        Task<EventInstanceDto> CreateInstanceAsync(
            EventInstanceCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EventInstanceDto>> ListInstancesAsync(
            CancellationToken cancellationToken = default);

        // ---------- Reward Rules ----------

        Task<EventRewardRuleDto> CreateRewardRuleAsync(
            EventRewardRuleCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EventRewardRuleDto>> ListRewardRulesAsync(
            Guid eventDefinitionId,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Simple stub implementation – wire to Application layer later.
    /// </summary>
    public sealed class EventApiService : IEventApiService
    {
        public Task<EventDefinitionDto?> GetDefinitionByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<EventDefinitionDto>> ListDefinitionsAsync(
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<EventDefinitionDto> CreateDefinitionAsync(
            EventDefinitionCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<EventDefinitionDto?> UpdateDefinitionAsync(
            Guid id,
            EventDefinitionUpdateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<EventInstanceDto> CreateInstanceAsync(
            EventInstanceCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<EventInstanceDto>> ListInstancesAsync(
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<EventRewardRuleDto> CreateRewardRuleAsync(
            EventRewardRuleCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<EventRewardRuleDto>> ListRewardRulesAsync(
            Guid eventDefinitionId,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}