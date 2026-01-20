using System.ComponentModel.DataAnnotations;

namespace RewardSystem_API.DTOs.Auth;

public class RegisterRequestDto
{
	[Required]
	public required string Name { get; set; }

	[Required, EmailAddress]
	public required string Email { get; set; }

	[Required]
	public required string EmployeeId { get; set; }

	[Required]
	public required string Password { get; set; }

	public string Role { get; set; } = "User";
}
