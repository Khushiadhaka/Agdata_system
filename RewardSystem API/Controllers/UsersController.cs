using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.User;
using RewardSystem_API.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
    /// <summary>
    /// Manages users, their profiles, and their reward accounts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserApiService _users;

        public UsersController(IUserApiService users)
        {
            _users = users;
        }

        // ---------------- USERS ----------------

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var list = await _users.ListAsync(ct);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var user = await _users.GetByIdAsync(id, ct);
            if (user is null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] UserCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _users.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UserUpdateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _users.UpdateAsync(id, dto, ct);
            if (updated is null)
                return NotFound(new { message = "User not found." });

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _users.DeleteAsync(id, ct);
            // If you want to return 404 when not found, you can change the service later.
            return NoContent();
        }

        // ---------------- PROFILE ----------------

        [HttpGet("{id:guid}/profile")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(Guid id, CancellationToken ct)
        {
            var profile = await _users.GetProfileAsync(id, ct);
            if (profile is null)
                return NotFound(new { message = "Profile not found." });

            return Ok(profile);
        }

        [HttpPut("{id:guid}/profile")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpsertProfile(
            Guid id,
            [FromBody] UserProfileCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profile = await _users.UpsertProfileAsync(id, dto, ct);
            return Ok(profile);
        }

        // ---------------- ACCOUNT ----------------

        [HttpGet("{id:guid}/account")]
        [ProducesResponseType(typeof(UserAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccount(Guid id, CancellationToken ct)
        {
            var account = await _users.GetAccountAsync(id, ct);
            if (account is null)
                return NotFound(new { message = "Account not found." });

            return Ok(account);
        }

        [HttpPost("{id:guid}/account/operate")]
        [ProducesResponseType(typeof(UserAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OperateOnAccount(
            Guid id,
            [FromBody] UserAccountOperationDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _users.ApplyOperationAsync(id, dto, ct);
            if (account is null)
                return BadRequest(new { message = "Operation could not be applied." });

            return Ok(account);
        }
    }
}
