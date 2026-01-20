namespace RewardSystem_Application.Interfaces.Auth
{
	public interface IAuthService
	{
		Task<string> LoginAsync(
			string email,
			string password,
			CancellationToken ct = default);

		Task<string> RegisterAsync(
			string name,
			string email,
			string employeeId,
			string password,
			string role = "User",
			CancellationToken ct = default);
	}
}
