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
	/// <summary>
	/// Central AutoMapper profile configuring mappings between Domain entities and DTOs.
	/// </summary>
	public sealed class MappingProfile : Profile
	{
		public MappingProfile()
		{
			MapUser();
			MapProduct();
			MapReward();
			MapRedemption();
			MapEvent();
			MapTransaction();
		}

		// ---------------- USER ----------------
		private void MapUser()
		{
			CreateMap<User, UserDto>()
				.ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
				.ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.EmployeeId.Value))
				.ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

			CreateMap<UserProfile, UserProfileDto>();

			CreateMap<UserAccount, UserAccountDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

			// ❌ Removed unsafe AuthResponseDto mapping
		}

		// ---------------- PRODUCT ----------------
		private void MapProduct()
		{
			CreateMap<Product, ProductDto>()
				.ForMember(
					d => d.SKU,
					opt => opt.MapFrom(s => s.Sku != null ? s.Sku.Value : null)
				);

			CreateMap<ProductInventory, ProductInventoryDto>();
		}

		// ---------------- REWARD ----------------
		private void MapReward()
		{
			CreateMap<Reward, RewardDto>()
				.ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

			CreateMap<RewardPoints, RewardPointsDto>();

			CreateMap<RewardTransaction, RewardTransactionDto>()
				.ForMember(d => d.TransactionType, opt => opt.MapFrom(s => s.TransactionType.ToString()));
		}

		// ---------------- REDEMPTION ----------------
		private void MapRedemption()
		{
			CreateMap<RedemptionRequest, RedemptionRequestDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

			CreateMap<RedemptionRecord, RedemptionRecordDto>();

			CreateMap<RedemptionProcess, RedemptionProcessDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
		}

		// ---------------- EVENT ----------------
		private void MapEvent()
		{
			CreateMap<EventDefinition, EventDefinitionDto>();
			CreateMap<EventInstance, EventInstanceDto>();
			CreateMap<EventRewardRule, EventRewardRuleDto>();
		}

		// ---------------- TRANSACTION ----------------
		private void MapTransaction()
		{
			CreateMap<Transaction, TransactionDto>()
				.ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
		}
	}
}
