using System;
using System.ComponentModel.DataAnnotations;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Redemption
{
	public sealed class RedemptionRequestUpdateDto
	{
		[Required]
		public RedemptionStatus Status { get; set; }

		public string? Note { get; set; }
	}
}
