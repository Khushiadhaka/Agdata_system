using API.Api.Server.DTOs.User;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using RewardSystem_Infrastructure.Infrastructure.Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // => "/api/users"
    public class UsersController : ControllerBase
    {
        private readonly RewardDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(RewardDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponseDto>>(users);
            return Ok(result);
        }

        // GET: /api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            var result = _mapper.Map<UserResponseDto>(user);
            return Ok(result);
        }

        // POST: /api/users
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = new User(
                    dto.Name,
                    dto.Email,
                    dto.EmployeeId,
                    (UserRole)dto.Role
                );

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<UserResponseDto>(user);

                return CreatedAtAction(nameof(GetUser),
                    new { id = result.Id },
                    result);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: /api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            try
            {
                user.Update(dto.Name, dto.Email, (UserRole)dto.Role);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: /api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            try
            {
                user.Delete();      // IsDeleted = true
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
