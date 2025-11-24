using Rewardsystem_Domain.Domain.Enums;

namespace API.Api.Server.DTOs.Reward
{
    public class CreateRewardDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RewardType Type { get; set; }
    }

    public class UpdateRewardDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RewardType Type { get; set; }
        public bool IsActive { get; set; }
    }

    public class RewardResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RewardType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
