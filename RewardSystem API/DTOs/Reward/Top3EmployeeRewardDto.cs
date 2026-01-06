namespace RewardSystem_API.DTOs.Reward
{
    // Top 3 users with highest reward points.
    public sealed class Top3EmployeeRewardDto
    {
        public Guid UserId { get; set; }       // Employee id
        public string Name { get; set; } = ""; // Employee name
        public int TotalPoints { get; set; }    // Total reward points earned
    }
}
