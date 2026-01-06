
using System;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Users;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Services
{
    // Manage user profile creation, update and removal.
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _profileRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;

        public UserProfileService(IUserProfileRepository profileRepo, IUserRepository userRepo, IUnitOfWork uow)
        {
            _profileRepo = profileRepo;
            _userRepo = userRepo;
            _uow = uow;
        }

        public async Task<UserProfile> CreateOrUpdateAsync(
            Guid userId,
            string phoneNumber,
            string department,
            string location,
            CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ValidationException("PhoneNumber required.");
            if (string.IsNullOrWhiteSpace(department)) throw new ValidationException("Department required.");
            if (string.IsNullOrWhiteSpace(location)) throw new ValidationException("Location required.");

            var user = await _userRepo.GetByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found.");
            if (user.IsDeleted) throw new BusinessRuleException("Cannot create profile for deleted user.");

            var existing = await _profileRepo.GetByUserIdAsync(userId, ct);
            if (existing == null)
            {
                var profile = new UserProfile(userId, phoneNumber.Trim(), department.Trim(), location.Trim());
                await _profileRepo.AddAsync(profile, ct);
                await _uow.SaveChangesAsync(ct);
                return profile;
            }

            existing.Update(phoneNumber.Trim(), department.Trim(), location.Trim());
            await _profileRepo.UpdateAsync(existing, ct);
            await _uow.SaveChangesAsync(ct);
            return existing;
        }

        public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) return null;
            return await _profileRepo.GetByUserIdAsync(userId, ct);
        }

        public async Task<bool> DeleteProfileAsync(Guid userId, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");

            var profile = await _profileRepo.GetByUserIdAsync(userId, ct);
            if (profile == null) return false;

            await _profileRepo.RemoveAsync(profile, ct);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
