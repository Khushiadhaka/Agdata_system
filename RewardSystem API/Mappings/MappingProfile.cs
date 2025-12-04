using AutoMapper;
using RewardSystem_API.DTOs.Auth;
using RewardSystem_API.DTOs.Event;
using RewardSystem_API.DTOs.Product;
using RewardSystem_API.DTOs.Redemption;
using RewardSystem_API.DTOs.Reward;
using RewardSystem_API.DTOs.Transaction;
using RewardSystem_API.DTOs.User;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace RewardSystem_API.Mappings
{
    // Central AutoMapper profile configuring mappings between Domain entities and DTOs.
    public sealed class MappingProfile : Profile
    {
        // Configure all mappings in constructor.
        public MappingProfile()
        {
            MapUser();
            MapProduct();
            MapReward();
            MapRedemption();
            MapEvent();
            MapTransaction();
        }

        // Configure mappings for User related DTOs.
        private void MapUser()
        {
            // User → UserDto (Email / EmployeeId from value objects).
            CreateMap<User, UserDto>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.EmployeeId.Value))
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // UserProfile → UserProfileDto.
            CreateMap<UserProfile, UserProfileDto>();

            // UserAccount → UserAccountDto.
            CreateMap<UserAccount, UserAccountDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            // Auth: Domain User → AuthResponseDto (only properties that actually exist on AuthResponseDto).
            // We keep it simple so it compiles regardless of the AuthResponseDto shape.
            CreateMap<User, AuthResponseDto>();
        }

        // Configure mappings for Product + Inventory.
        private void MapProduct()
        {
            // Product → ProductDto.
            CreateMap<Product, ProductDto>();

            // ProductInventory → ProductInventoryDto.
            CreateMap<ProductInventory, ProductInventoryDto>();
        }

        // Configure mappings for Reward module.
        private void MapReward()
        {
            // Reward → RewardDto.
            CreateMap<Reward, RewardDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

            // RewardPoints → RewardPointsDto.
            CreateMap<RewardPoints, RewardPointsDto>();

            // RewardTransaction → RewardTransactionDto.
            CreateMap<RewardTransaction, RewardTransactionDto>()
                .ForMember(d => d.TransactionType, opt => opt.MapFrom(s => s.TransactionType.ToString()));

            // Custom for Top3EmployeeRewardDto will usually be from SQL projection (manual mapping).
        }

        // Configure mappings for Redemption module.
        private void MapRedemption()
        {
            // RedemptionRequest → RedemptionRequestDto.
            CreateMap<RedemptionRequest, RedemptionRequestDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            // RedemptionRecord → RedemptionRecordDto.
            CreateMap<RedemptionRecord, RedemptionRecordDto>();

            // RedemptionProcess → RedemptionProcessDto.
            CreateMap<RedemptionProcess, RedemptionProcessDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
        }

        // Configure mappings for Event module.
        private void MapEvent()
        {
            // EventDefinition → EventDefinitionDto.
            CreateMap<EventDefinition, EventDefinitionDto>();

            // EventInstance → EventInstanceDto.
            CreateMap<EventInstance, EventInstanceDto>();

            // EventRewardRule → EventRewardRuleDto.
            CreateMap<EventRewardRule, EventRewardRuleDto>();
        }

        // Configure mappings for Transaction module.
        private void MapTransaction()
        {
            // Transaction → TransactionDto.
            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
        }
    }
}
