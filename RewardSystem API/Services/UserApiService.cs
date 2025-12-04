using AutoMapper;
using RewardSystem_API.DTOs.User;
using RewardSystem_Application.Interfaces.Users;


namespace RewardSystem_API.Services
{
    /// <summary>
    /// API-level service used by <see cref="Controllers.UsersController"/> to
    /// work with users, profiles and accounts.
    /// 
    /// NOTE:
    /// Right now this class only contains stub / placeholder implementations
    /// so that the project compiles. You can later replace the bodies with
    /// real calls to your application layer / repositories.
    /// </summary>
    public sealed class UserApiService : IUserApiService
    {
        
        // 1. Users
        

        /// <summary>
        /// Returns a list of users.
        /// Currently returns an empty list (stub).
        /// </summary>
        public Task<IReadOnlyList<UserDto>> ListAsync(
            CancellationToken cancellationToken = default)
        {
            
            IReadOnlyList<UserDto> empty = Array.Empty<UserDto>();
            return Task.FromResult(empty);
        }

        /// <summary>
        /// Returns a single user by id.
        /// Currently always returns null (stub).
        /// </summary>
        public Task<UserDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            
            return Task.FromResult<UserDto?>(null);
        }

        /// <summary>
        /// Creates a new user.
        /// Currently throws NotImplementedException (stub).
        /// </summary>
        public Task<UserDto> CreateAsync(
            UserCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            
            throw new NotImplementedException(
                "UserApiService.CreateAsync is not wired yet. " +
                "Call your application services / repositories here.");
        }

        /// <summary>
        /// Updates an existing user.
        /// Currently throws NotImplementedException (stub).
        /// </summary>
        public Task<UserDto?> UpdateAsync(
            Guid id,
            UserUpdateDto dto,
            CancellationToken cancellationToken = default)
        {
            // TODO: Replace with real implementation
            throw new NotImplementedException(
                "UserApiService.UpdateAsync is not wired yet.");
        }

        /// <summary>
        /// Deletes a user.
        /// Currently does nothing (stub).
        /// </summary>
        public Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            
            // For now just return completed task so code compiles.
            return Task.CompletedTask;
        }

        
        // 2. Profile
        

        /// <summary>
        /// Gets the profile for a specific user.
        /// Currently returns null (stub).
        /// </summary>
        public Task<UserProfileDto?> GetProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            // TODO: Replace with real implementation
            return Task.FromResult<UserProfileDto?>(null);
        }

        /// <summary>
        /// Creates or updates a profile for a user.
        /// Currently throws NotImplementedException (stub).
        /// </summary>
        public Task<UserProfileDto> UpsertProfileAsync(
            Guid userId,
            UserProfileCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            // TODO: Replace with real implementation
            throw new NotImplementedException(
                "UserApiService.UpsertProfileAsync is not wired yet.");
        }

        // 3. Account / points
       

        /// <summary>
        /// Returns the account (points balance) for a user.
        /// Currently returns null (stub).
        /// </summary>
        public Task<UserAccountDto?> GetAccountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<UserAccountDto?>(null);
        }

        /// <summary>
        /// Applies an operation to a user account (add / deduct / set).
        /// Currently throws NotImplementedException (stub).
        /// </summary>
        public Task<UserAccountDto?> ApplyOperationAsync(
            Guid userId,
            UserAccountOperationDto dto,
            CancellationToken cancellationToken = default)
        {
            
            throw new NotImplementedException(
                "UserApiService.ApplyOperationAsync is not wired yet.");
        }
    }
}