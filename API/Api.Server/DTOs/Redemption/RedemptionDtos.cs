using Rewardsystem_Domain.Domain.Enums;

namespace API.Api.Server.DTOs.Redemption
{
    public class CreateRedemptionRequestDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int PointsUsed { get; set; }
    }

    public class UpdateRedemptionPointsDto
    {
        public int PointsUsed { get; set; }
    }

    public class RedemptionRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int PointsUsed { get; set; }
        public RedemptionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
