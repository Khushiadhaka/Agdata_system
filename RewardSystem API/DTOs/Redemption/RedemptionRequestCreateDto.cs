using System;
using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Redemption
{
	public sealed class RedemptionRequestCreateDto
	{
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid ProductId { get; set; }

		[Range(1, int.MaxValue)]
		public int PointsUsed { get; set; }
	}
}

