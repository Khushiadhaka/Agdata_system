using RewardSystem_API.DTOs.User;

namespace RewardSystem_API.Services
{
    /// <summary>
    /// Contract for API-level operations related to users, profiles and accounts.
    /// Implemented by <see cref="UserApiService" /> and used by UsersController.
    /// </summary>
    public interface IUserApiService
    {
        // ----------------------- Users -----------------------

        /// <summary>
        /// Returns all users.
        /// </summary>
        Task<IReadOnlyList<UserDto>> ListAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a user by id.
        /// </summary>
        Task<UserDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        Task<UserDto> CreateAsync(
            UserCreateDto dto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        Task<UserDto?> UpdateAsync(
            Guid id,
            UserUpdateDto dto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        // ----------------------- Profile -----------------------

        /// <summary>
        /// Gets the profile for a given user.
        /// </summary>
        Task<UserProfileDto?> GetProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates or updates the profile for a user.
        /// </summary>
        Task<UserProfileDto> UpsertProfileAsync(
            Guid userId,
            UserProfileCreateDto dto,
            CancellationToken cancellationToken = default);

        // ----------------------- Account / points -----------------------

        /// <summary>
        /// Gets the account (points balance) for a user.
        /// </summary>
        Task<UserAccountDto?> GetAccountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Applies an operation (add / deduct / set) to a user account.
        /// </summary>
        Task<UserAccountDto?> ApplyOperationAsync(
            Guid userId,
            UserAccountOperationDto dto,
            CancellationToken cancellationToken = default);
    }
}
