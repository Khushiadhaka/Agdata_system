using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Xunit;

namespace RewardSystem_Test.Users
{
    public class UserTests
    {
        // -------------------- Constructor Tests --------------------

        [Fact]
        public void Constructor_ValidInputs_CreatesUserCorrectly()
        {
            // Act
            var user = new User("John Doe", "john@example.com", "E123", UserRole.Admin);

            // Assert
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("john@example.com", user.Email.Value);
            Assert.Equal("E123", user.EmployeeId.Value);
            Assert.Equal(UserRole.Admin, user.Role);
            Assert.False(user.IsDeleted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidName_Throws(string? name)
        {
            Assert.Throws<ValidationException>(() =>
                new User(name!, "a@a.com", "E1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidEmail_Throws(string? email)
        {
            Assert.Throws<ValidationException>(() =>
                new User("Test", email!, "E1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidEmployeeId_Throws(string? empId)
        {
            Assert.Throws<ValidationException>(() =>
                new User("Test", "a@a.com", empId!));
        }

        // -------------------- Update Tests --------------------

        [Fact]
        public void Update_ValidInputs_UpdatesUser()
        {
            var user = new User("Old Name", "old@mail.com", "E123");

            user.Update("New Name", "new@mail.com", UserRole.Admin);

            Assert.Equal("New Name", user.Name);
            Assert.Equal("new@mail.com", user.Email.Value);
            Assert.Equal(UserRole.Admin, user.Role);
            Assert.NotNull(user.UpdatedAt);
        }

        [Fact]
        public void Update_DeletedUser_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            user.Delete();

            Assert.Throws<BusinessRuleException>(() =>
                user.Update("New", "new@mail.com", UserRole.User));
        }

        [Fact]
        public void Update_InvalidName_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            Assert.Throws<ValidationException>(() =>
                user.Update("", "valid@mail.com", UserRole.User));
        }

        [Fact]
        public void Update_InvalidEmail_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            Assert.Throws<ValidationException>(() =>
                user.Update("Valid", "", UserRole.User));
        }

        // -------------------- Delete Tests --------------------

        [Fact]
        public void Delete_FirstTime_MarksUserAsDeleted()
        {
            var user = new User("John", "john@mail.com", "E1");

            user.Delete();

            Assert.True(user.IsDeleted);
            Assert.NotNull(user.UpdatedAt);
        }

        [Fact]
        public void Delete_AlreadyDeleted_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            user.Delete();

            Assert.Throws<BusinessRuleException>(() => user.Delete());
        }

        // -------------------- Attach Profile Tests --------------------

        [Fact]
        public void AttachProfile_ValidProfile_AttachesCorrectly()
        {
            var user = new User("John", "john@mail.com", "E1");
            var profile = new UserProfile(user.Id, "9999999999", "Engineering", "Delhi");

            user.AttachProfile(profile);

            Assert.Equal(profile, user.Profile);
        }

        [Fact]
        public void AttachProfile_Null_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            Assert.Throws<ValidationException>(() => user.AttachProfile(null!));
        }

        [Fact]
        public void AttachProfile_WrongUserId_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            var wrongProfile = new UserProfile(Guid.NewGuid(), "999", "Dept", "Loc");

            Assert.Throws<BusinessRuleException>(() => user.AttachProfile(wrongProfile));
        }

        // -------------------- Attach Account Tests --------------------

        [Fact]
        public void AttachAccount_ValidAccount_AttachesCorrectly()
        {
            var user = new User("John", "john@mail.com", "E1");
            var account = new UserAccount(user.Id);

            user.AttachAccount(account);

            Assert.Equal(account, user.Account);
        }

        [Fact]
        public void AttachAccount_Null_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            Assert.Throws<ValidationException>(() => user.AttachAccount(null!));
        }

        [Fact]
        public void AttachAccount_WrongUserId_Throws()
        {
            var user = new User("John", "john@mail.com", "E1");
            var wrongAccount = new UserAccount(Guid.NewGuid());

            Assert.Throws<BusinessRuleException>(() => user.AttachAccount(wrongAccount));
        }
    }
}
