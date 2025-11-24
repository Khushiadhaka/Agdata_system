using API.Api.Server.DTOs.Event;
using API.Api.Server.DTOs.Product;
using API.Api.Server.DTOs.Redemption;
using API.Api.Server.DTOs.Reward;
using API.Api.Server.DTOs.Transactions;
using API.Api.Server.DTOs.User;
using AutoMapper;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace API.Api.Server.Mappings
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // User
            CreateMap<User, UserResponseDto>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            CreateMap<CreateUserDto, User>()
                .ConstructUsing(src => new User(src.Name, src.Email, src.EmployeeId, (Rewardsystem_Domain.Domain.Enums.UserRole)src.Role));

            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Product
            CreateMap<Product, ProductResponseDto>();
            CreateMap<CreateProductDto, Product>()
                .ConstructUsing(src => new Product(src.Name, src.Description, src.RequiredPoints));
            CreateMap<UpdateProductDto, Product>();

            // RedemptionRequest
            CreateMap<RedemptionRequest, RedemptionRequestResponseDto>();
            CreateMap<CreateRedemptionRequestDto, RedemptionRequest>()
                .ConstructUsing(src => new RedemptionRequest(src.UserId, src.ProductId, src.PointsUsed));

            // Reward
            CreateMap<Reward, RewardResponseDto>();
            CreateMap<CreateRewardDto, Reward>()
                .ConstructUsing(src => new Reward(src.Name, src.Description, src.Type));

            // EventDefinition / Instance
            CreateMap<EventDefinition, EventDefinitionResponseDto>();
            CreateMap<CreateEventDefinitionDto, EventDefinition>()
                .ConstructUsing(src => new EventDefinition(src.Name, src.Description, src.RewardPoints));

            CreateMap<EventInstance, EventInstanceResponseDto>();
            CreateMap<CreateEventInstanceDto, EventInstance>()
                .ConstructUsing(src => new EventInstance(src.EventDefinitionId, src.StartTime, src.EndTime));

            // Transaction
            CreateMap<Transaction, TransactionResponseDto>();
            CreateMap<CreateTransactionDto, Transaction>()
                .ConstructUsing(src => new Transaction(src.UserId, src.ProductId, src.Amount, src.RewardPointsEarned, src.Type));
        }
    }
}
