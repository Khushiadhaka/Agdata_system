using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Users
{
    public sealed class UserTests
    {
        [Fact]
        public void Ctor_Should_Create_Valid_User()
        {
            var user = new User("Name", "test@abc.com", "E001", UserRole.Employee);

            user.Name.Should().Be("Name");
            user.Email.Should().Be("test@abc.com");
            user.EmployeeId.Should().Be("E001");
            user.Role.Should().Be(UserRole.Employee);
            user.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Name_Empty()
        {
            Action act = () => new User("", "a@a.com", "E1", UserRole.Employee);
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Update_Should_Throw_When_Deleted()
        {
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);
            user.Delete();

            Action act = () => user.Update("New", "b@b.com", UserRole.Admin);
            act.Should().Throw<BusinessRuleException>();
        }

        [Fact]
        public void AttachAccount_Should_Throw_When_UserId_Different()
        {
            var user = new User("Name", "a@a.com", "E1", UserRole.Employee);
            var account = new UserAccount(Guid.NewGuid()); // diff id

            Action act = () => user.AttachAccount(account);
            act.Should().Throw<BusinessRuleException>();
        }
    }
}
