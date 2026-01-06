using System.ComponentModel.DataAnnotations;

using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_API.DTOs.Redemption
{
    // Payload used to change the status of an existing redemption request.
    public sealed class RedemptionRequestUpdateDto
    {
        // Id of the redemption request to update.
        public Guid RequestId { get; set; }

        // New status for the request.
        public string Status { get; set; } = string.Empty;

        // Optional note or comment about this status change.
        public string? Note { get; set; }
    }
}
