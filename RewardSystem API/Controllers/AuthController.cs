using Microsoft.AspNetCore.Mvc;
using RewardSystem_Application.Interfaces.Auth;

namespace RewardSystem_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var token = await _authService.LoginAsync(
                request.Email,
                request.Password,
                ct);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            CancellationToken ct)
        {
            var token = await _authService.RegisterAsync(
                request.Name,
                request.Email,
                request.EmployeeId,
                request.Password,
                request.Role ?? "User",
                ct);

            return Ok(new { token });
        }
    }

    // ---------------- DTOs ----------------

    public sealed class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
