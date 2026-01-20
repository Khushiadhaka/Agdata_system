using RewardSystem_API.DTOs.Redemption;

namespace RewardSystem_API.Services
{
	public interface IRedemptionApiService
	{
		// ----- Requests -----

		Task<RedemptionRequestDto> CreateRequestAsync(
			RedemptionRequestCreateDto dto,
			CancellationToken ct = default);

		Task<RedemptionRequestDto> UpdateRequestAsync(
			Guid requestId,
			RedemptionRequestUpdateDto dto,
			CancellationToken ct = default);

		Task<IReadOnlyList<RedemptionRequestDto>> ListRequestsByUserAsync(
			Guid userId,
			CancellationToken ct = default);

		// ----- Records -----

		Task<IReadOnlyList<RedemptionRecordDto>> ListRecordsByUserAsync(
			Guid userId,
			CancellationToken ct = default);
	}
}

