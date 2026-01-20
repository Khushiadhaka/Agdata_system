using AutoMapper;
using RewardSystem_API.DTOs.User;
using RewardSystem_Application.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Services
{
	/// <summary>
	/// API-level service used by UsersController.
	/// This class orchestrates application services and maps domain → DTO.
	/// </summary>
	public sealed class UserApiService : IUserApiService
	{
		private readonly IUserService _userService;
		private readonly IUserProfileService _profileService;
		private readonly IUserAccountService _accountService;
		private readonly IMapper _mapper;

		public UserApiService(
			IUserService userService,
			IUserProfileService profileService,
			IUserAccountService accountService,
			IMapper mapper)
		{
			_userService = userService;
			_profileService = profileService;
			_accountService = accountService;
			_mapper = mapper;
		}

		// ---------------- USERS ----------------

		public async Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct = default)
		{
			var users = await _userService.GetAllAsync(ct);
			return _mapper.Map<IReadOnlyList<UserDto>>(users);
		}

		public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			var user = await _userService.GetByIdAsync(id, ct);
			return user == null ? null : _mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken ct = default)
		{
			var user = await _userService.CreateUserAsync(
				dto.Name,
				dto.Email,
				dto.EmployeeId,
				dto.Role,
				ct);

			return _mapper.Map<UserDto>(user);
		}

		public async Task<UserDto?> UpdateAsync(Guid id, UserUpdateDto dto, CancellationToken ct = default)
		{
			var user = await _userService.UpdateUserAsync(
				id,
				dto.Name,
				dto.Email,
				dto.Role,
				ct);

			return _mapper.Map<UserDto>(user);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct = default)
		{
			await _userService.DeleteUserAsync(id, ct);
		}

		// ---------------- PROFILE ----------------

		public async Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct = default)
		{
			var profile = await _profileService.GetByUserIdAsync(userId, ct);
			return profile == null ? null : _mapper.Map<UserProfileDto>(profile);
		}

		public async Task<UserProfileDto> UpsertProfileAsync(
			Guid userId,
			UserProfileCreateDto dto,
			CancellationToken ct = default)
		{
			var profile = await _profileService.CreateOrUpdateAsync(
				userId,
				dto.PhoneNumber,
				dto.Department,
				dto.Location,
				ct);

			return _mapper.Map<UserProfileDto>(profile);
		}

		// ---------------- ACCOUNT / POINTS ----------------

		public async Task<UserAccountDto?> GetAccountAsync(Guid userId, CancellationToken ct = default)
		{
			var account = await _accountService.GetAccountAsync(userId, ct);
			return account == null ? null : _mapper.Map<UserAccountDto>(account);
		}

		public async Task<UserAccountDto?> ApplyOperationAsync(
			Guid userId,
			UserAccountOperationDto dto,
			CancellationToken ct = default)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			switch (dto.Operation)
			{
				case UserAccountOperation.Add:
					await _accountService.AddPointsAsync(userId, dto.Points, dto.Reference, ct);
					break;

				case UserAccountOperation.Deduct:
					var success = await _accountService.TryDeductPointsAsync(
						userId, dto.Points, dto.Reference, ct);

					if (!success)
						return null;
					break;

				case UserAccountOperation.Set:
					// 🔥 IMPORTANT:
					// SetPoints is intentionally removed from domain.
					// We convert absolute value → delta internally.
					await _accountService.AdjustPointsAsync(
						userId, dto.Points, dto.Reference, ct);
					break;

				default:
					throw new InvalidOperationException("Unsupported account operation.");
			}

			var updated = await _accountService.GetAccountAsync(userId, ct);
			return updated == null ? null : _mapper.Map<UserAccountDto>(updated);
		}
	}
}
