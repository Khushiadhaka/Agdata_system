using AutoMapper;
using RewardSystem_API.DTOs.Redemption;
using RewardSystem_Application.Interfaces.Redemption;


namespace RewardSystem_API.Services
{
    // High-level API service used by controllers for redemption related operations.
    public interface IRedemptionApiService
    {
        Task<RedemptionRequestDto> CreateRequestAsync(
            RedemptionRequestCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<RedemptionRequestDto?> UpdateRequestAsync(
            Guid requestId,
            RedemptionRequestUpdateDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RedemptionRequestDto>> ListRequestsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RedemptionRecordDto>> ListRecordsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<RedemptionProcessDto?> GetProcessAsync(
            Guid requestId,
            CancellationToken cancellationToken = default);
    }

    // Concrete implementation that talks to application layer services.
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

        /// <summary>
        /// Creates a new redemption request for a user.
        /// </summary>
        public async Task<RedemptionRequestDto> CreateRequestAsync(
            RedemptionRequestCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            // Signature we know exists:
            // IRedemptionRequestService.CreateAsync(Guid userId, Guid productId, int pointsUsed, CancellationToken ct)
            var request = await _requestService.CreateAsync(
                dto.UserId,
                dto.ProductId,
                dto.PointsUsed,
                cancellationToken);

            return _mapper.Map<RedemptionRequestDto>(request);
        }

        /// <summary>
        /// Updates an existing redemption request.
        /// NOTE: Not wired to domain service yet – adjust when you decide the proper domain method.
        /// </summary>
        public Task<RedemptionRequestDto?> UpdateRequestAsync(
            Guid requestId,
            RedemptionRequestUpdateDto dto,
            CancellationToken cancellationToken = default)
        {
            // TODO: connect to IRedemptionRequestService when update method is defined.
            throw new NotImplementedException("UpdateRequestAsync is not wired to domain service yet.");
        }

        /// <summary>
        /// Lists all redemption requests for a given user.
        /// </summary>
        public async Task<IReadOnlyList<RedemptionRequestDto>> ListRequestsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var items = await _requestService.ListByUserAsync(userId, cancellationToken);
            return items
                .Select(x => _mapper.Map<RedemptionRequestDto>(x))
                .ToList();
        }

        /// <summary>
        /// Lists all completed redemption records for a given user.
        /// </summary>
        public async Task<IReadOnlyList<RedemptionRecordDto>> ListRecordsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var items = await _recordService.ListByUserAsync(userId, cancellationToken);
            return items
                .Select(x => _mapper.Map<RedemptionRecordDto>(x))
                .ToList();
        }

        /// <summary>
        /// Returns processing details for a specific redemption request.
        
        /// </summary>
        public Task<RedemptionProcessDto?> GetProcessAsync(
            Guid requestId,
            CancellationToken cancellationToken = default)
        {
            // connect to IRedemptionProcessService when correct method is available.
            throw new NotImplementedException("GetProcessAsync is not wired to domain service yet.");
        }
    }
}
