using AutoMapper;
using RewardSystem_API.DTOs.Event;
using RewardSystem_Application.Interfaces.Event;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Services
{
	/// <summary>
	/// API-level service that wires EventController to Application services.
	/// </summary>
	public sealed class EventApiService : IEventApiService
	{
		private readonly IEventDefinitionService _definitionService;
		private readonly IEventInstanceService _instanceService;
		private readonly IEventRewardRuleService _ruleService;
		private readonly IMapper _mapper;

		public EventApiService(
			IEventDefinitionService definitionService,
			IEventInstanceService instanceService,
			IEventRewardRuleService ruleService,
			IMapper mapper)
		{
			_definitionService = definitionService;
			_instanceService = instanceService;
			_ruleService = ruleService;
			_mapper = mapper;
		}

		// ================= EVENT DEFINITIONS =================

		public async Task<EventDefinitionDto?> GetDefinitionByIdAsync(
			Guid id,
			CancellationToken cancellationToken = default)
		{
			var def = await _definitionService.GetByIdAsync(id, cancellationToken);
			return def == null ? null : _mapper.Map<EventDefinitionDto>(def);
		}

		public async Task<IReadOnlyList<EventDefinitionDto>> ListDefinitionsAsync(
			CancellationToken cancellationToken = default)
		{
			var list = await _definitionService.ListAsync(false, cancellationToken);
			return _mapper.Map<IReadOnlyList<EventDefinitionDto>>(list);
		}

		public async Task<EventDefinitionDto> CreateDefinitionAsync(
			EventDefinitionCreateDto dto,
			CancellationToken cancellationToken = default)
		{
			var def = await _definitionService.CreateAsync(
				dto.Name,
				dto.Description,
				dto.RewardPoints,
				cancellationToken);

			return _mapper.Map<EventDefinitionDto>(def);
		}

		public async Task<EventDefinitionDto?> UpdateDefinitionAsync(
			Guid id,
			EventDefinitionUpdateDto dto,
			CancellationToken cancellationToken = default)
		{
			var def = await _definitionService.UpdateAsync(
				id,
				dto.Name,
				dto.Description,
				dto.RewardPoints,
				cancellationToken);

			return _mapper.Map<EventDefinitionDto>(def);
		}

		// ================= EVENT INSTANCES =================

		public async Task<EventInstanceDto> CreateInstanceAsync(
			EventInstanceCreateDto dto,
			CancellationToken cancellationToken = default)
		{
			var instance = await _instanceService.CreateAsync(
				dto.EventDefinitionId,
				dto.StartTime,
				dto.EndTime,
				cancellationToken);

			return _mapper.Map<EventInstanceDto>(instance);
		}

		public Task<IReadOnlyList<EventInstanceDto>> ListInstancesAsync(
			CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException(
				"Global instance listing not supported yet.");
		}

		// ================= REWARD RULES =================

		public async Task<EventRewardRuleDto> CreateRewardRuleAsync(
			EventRewardRuleCreateDto dto,
			CancellationToken cancellationToken = default)
		{
			var rule = await _ruleService.CreateAsync(
				dto.EventDefinitionId,
				dto.Condition,
				dto.Points,
				cancellationToken);

			return _mapper.Map<EventRewardRuleDto>(rule);
		}

		public async Task<IReadOnlyList<EventRewardRuleDto>> ListRewardRulesAsync(
			Guid eventDefinitionId,
			CancellationToken cancellationToken = default)
		{
			var rules = await _ruleService.GetByDefinitionAsync(
				eventDefinitionId,
				cancellationToken);

			return _mapper.Map<IReadOnlyList<EventRewardRuleDto>>(rules);
		}
	}
}
