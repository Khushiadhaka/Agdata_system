using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Xunit;

namespace RewardSystem_Test.Domain.Products
{
    public sealed class ProductTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Product_With_Valid_Data()
        {
            // Arrange
            var name = "Gift Voucher";
            var description = "Amazon gift voucher";
            var points = 500;

            // Act
            var product = new Product(name, description, points);

            // Assert
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.RequiredPoints.Should().Be(points);
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Name_Is_Empty()
        {
            // Act
            Action act = () => new Product("", "desc", 100);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Product name cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_RequiredPoints_Not_Positive()
        {
            // Act
            Action act = () => new Product("Name", "desc", 0);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Required points must be greater than zero*");
        }

        [Fact]
        public void Update_Should_Change_All_Fields_When_Active()
        {
            // Arrange
            var product = new Product("Old", "Old desc", 100);

            // Act
            product.Update("New", "New desc", 200);

            // Assert
            product.Name.Should().Be("New");
            product.Description.Should().Be("New desc");
            product.RequiredPoints.Should().Be(200);
        }

        [Fact]
        public void Update_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            var product = new Product("Old", "Old desc", 100);

            // Act
            Action act = () => product.Update("", "New desc", 200);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Product name cannot be empty*");
        }

        [Fact]
        public void Update_Should_Throw_When_RequiredPoints_Not_Positive()
        {
            // Arrange
            var product = new Product("Old", "Old desc", 100);

            // Act
            Action act = () => product.Update("New", "New desc", 0);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*greater than zero*");
        }

        [Fact]
        public void Update_Should_Throw_When_Product_Is_Inactive()
        {
            // Arrange
            var product = new Product("Old", "Old desc", 100);
            product.Deactivate();

            // Act
            Action act = () => product.Update("New", "New desc", 200);

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive product*");
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            // Arrange
            var product = new Product("Name", "Desc", 100);

            // Act
            product.Deactivate();

            // Assert
            product.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_Should_Throw_When_Already_Inactive()
        {
            // Arrange
            var product = new Product("Name", "Desc", 100);
            product.Deactivate();

            // Act
            Action act = () => product.Deactivate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already inactive*");
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True_When_Currently_Inactive()
        {
            // Arrange
            var product = new Product("Name", "Desc", 100);
            product.Deactivate();

            // Act
            product.Activate();

            // Assert
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Activate_Should_Throw_When_Already_Active()
        {
            // Arrange
            var product = new Product("Name", "Desc", 100);

            // Act
            Action act = () => product.Activate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already active*");
        }
    }
}

