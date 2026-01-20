using AutoMapper;
using RewardSystem_API.DTOs.Redemption;
using RewardSystem_Application.Interfaces.Redemption;

namespace RewardSystem_API.Services
{
	/// <summary>
	/// API-layer service for Redemption module.
	/// Orchestrates DTOs ↔ Application services.
	/// </summary>
	public sealed class RedemptionApiService : IRedemptionApiService
	{
		private readonly IRedemptionRequestService _requestService;
		private readonly IRedemptionProcessService _processService;
		private readonly IRedemptionRecordService _recordService;
		private readonly IMapper _mapper;

		public RedemptionApiService(
			IRedemptionRequestService requestService,
			IRedemptionProcessService processService,
			IRedemptionRecordService recordService,
			IMapper mapper)
		{
			_requestService = requestService;
			_processService = processService;
			_recordService = recordService;
			_mapper = mapper;
		}

		// ---------------- REQUESTS ----------------

		public async Task<RedemptionRequestDto> CreateRequestAsync(
			RedemptionRequestCreateDto dto,
			CancellationToken ct = default)
		{
			var request = await _requestService.CreateAsync(
				dto.UserId,
				dto.ProductId,
				dto.PointsUsed,
				ct);

			return _mapper.Map<RedemptionRequestDto>(request);
		}

		public async Task<RedemptionRequestDto> UpdateRequestAsync(
			Guid requestId,
			RedemptionRequestUpdateDto dto,
			CancellationToken ct = default)
		{
			var updated = await _requestService.UpdateStatusAsync(
				requestId,
				dto.Status,
				dto.Note,
				ct);

			// ✅ Auto-complete flow when approved → completed
			if (dto.Status == Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Completed)
			{
				await _processService.CompleteRequestAsync(requestId, dto.Note, ct);
			}

			return _mapper.Map<RedemptionRequestDto>(updated);
		}

		public async Task<IReadOnlyList<RedemptionRequestDto>> ListRequestsByUserAsync(
			Guid userId,
			CancellationToken ct = default)
		{
			var list = await _requestService.ListByUserAsync(userId, ct);
			return _mapper.Map<IReadOnlyList<RedemptionRequestDto>>(list);
		}

		// ---------------- RECORDS ----------------

		public async Task<IReadOnlyList<RedemptionRecordDto>> ListRecordsByUserAsync(
			Guid userId,
			CancellationToken ct = default)
		{
			var records = await _recordService.ListByUserAsync(userId, ct);
			return _mapper.Map<IReadOnlyList<RedemptionRecordDto>>(records);
		}
	}
}
