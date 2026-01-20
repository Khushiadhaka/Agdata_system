using RewardSystem_API.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Services
{
	public interface IEventApiService
	{
		// Event Definitions
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

		// Event Instances
		Task<EventInstanceDto> CreateInstanceAsync(
			EventInstanceCreateDto dto,
			CancellationToken cancellationToken = default);

		Task<IReadOnlyList<EventInstanceDto>> ListInstancesAsync(
			CancellationToken cancellationToken = default);

		// Reward Rules
		Task<EventRewardRuleDto> CreateRewardRuleAsync(
			EventRewardRuleCreateDto dto,
			CancellationToken cancellationToken = default);

		Task<IReadOnlyList<EventRewardRuleDto>> ListRewardRulesAsync(
			Guid eventDefinitionId,
			CancellationToken cancellationToken = default);
	}
}

