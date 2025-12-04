
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Auth;
using RewardSystem_Application.Interfaces.Auth;

namespace Api.Server.Controllers;

/// <summary>
/// Handles authentication operations such as user login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    // Application-layer auth service (already implemented in your Application project)
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor with DI-injected auth service.
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    /// <summary>
    /// Authenticates a user and returns a JWT access token.
    /// </summary>
    /// <param name="dto">Login credentials (email + password).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>JWT token if login succeeds.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/login
    ///     {
    ///       "email": "admin@agdata.com",
    ///       "password": "Admin@123"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns a valid JWT token.</response>
    /// <response code="400">If the model is invalid.</response>
    /// <response code="401">If credentials are invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken ct)
    {
        // Validate model (required fields, email etc.)
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // NOTE:
        // Your current IAuthService.LoginAsync returns a string (token) – that’s why
        // you were getting errors when accessing result.Success / result.Token etc.
        var token = await _authService.LoginAsync(dto.Email, dto.Password, ct);

        // If service returns null or empty => treat as invalid credentials.
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized(new { message = "Invalid email or password." });

        // Map to DTO – for now we only expose the token.
        var response = new AuthResponseDto
        {
            Token = token
        };

        return Ok(response);
    }
}

