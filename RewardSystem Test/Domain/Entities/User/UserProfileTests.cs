using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Xunit;

namespace RewardSystem_Test.Users
{
    public sealed class UserProfileTests
    {
        [Fact]
        public void Ctor_Should_Create_Profile_With_Valid_Data()
        {
            var profile = new UserProfile(
                Guid.NewGuid(),
                "9999999999",
                "Mechanical",
                "Delhi");

            profile.PhoneNumber.Should().Be("9999999999");
            profile.Department.Should().Be("Mechanical");
            profile.Location.Should().Be("Delhi");
        }

        [Fact]
        public void Update_Should_Throw_When_Phone_Empty()
        {
            var profile = new UserProfile(
                Guid.NewGuid(),
                "9999999999",
                "Mechanical",
                "Delhi");

            Action act = () => profile.Update("", "Mech", "Delhi");
            act.Should().Throw<ValidationException>();
        }
    }
}
