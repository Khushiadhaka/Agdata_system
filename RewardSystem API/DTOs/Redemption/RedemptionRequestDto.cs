using System;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Redemption
{
	public sealed class RedemptionRequestDto
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
		public int PointsUsed { get; set; }
		public RedemptionStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
