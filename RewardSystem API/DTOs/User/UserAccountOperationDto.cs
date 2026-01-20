using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.User
{
	/// <summary>
	/// Supported operations on a user account.
	/// </summary>
	public enum UserAccountOperation
	{
		Add = 1,
		Deduct = 2,
		Set = 3
	}

	/// <summary>
	/// Request payload for operating on a user's account.
	/// </summary>
	public sealed class UserAccountOperationDto
	{
		[Required]
		public UserAccountOperation Operation { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Points must be greater than 0.")]
		public int Points { get; set; }

		public string? Reference { get; set; }
	}
}
