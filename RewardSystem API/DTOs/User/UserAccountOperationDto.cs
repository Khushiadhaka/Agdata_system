using System.ComponentModel.DataAnnotations;
namespace RewardSystem_API.DTOs.User
{
    // Represents payload for operations on user account points.
    public sealed class UserAccountOperationDto
    {
        // Target user identifier.
        public Guid UserId { get; set; }

        // Points value (add/set/deduct defined by API endpoint).
        public int Points { get; set; }

        // Optional reference / description.
        public string? Reference { get; set; }
        public string Operation { get; set; }
    }
}
