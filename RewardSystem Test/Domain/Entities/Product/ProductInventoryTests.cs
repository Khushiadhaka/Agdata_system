using System;
using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Product;
using Xunit;

namespace RewardSystem_Test.Domain.Products
{
    public sealed class ProductInventoryTests
    {
        [Fact]
        public void Ctor_Should_Create_Active_Inventory_With_Valid_Data()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var stock = 10;

            // Act
            var inventory = new ProductInventory(productId, stock);

            // Assert
            inventory.ProductId.Should().Be(productId);
            inventory.StockQuantity.Should().Be(stock);
            inventory.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_When_ProductId_Empty()
        {
            // Act
            Action act = () => new ProductInventory(Guid.Empty, 10);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*ProductId cannot be empty*");
        }

        [Fact]
        public void Ctor_Should_Throw_When_Stock_Negative()
        {
            // Act
            Action act = () => new ProductInventory(Guid.NewGuid(), -1);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*Stock quantity cannot be negative*");
        }

        [Fact]
        public void IncreaseStock_Should_Add_Quantity_When_Active()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);

            // Act
            inv.IncreaseStock(3);

            // Assert
            inv.StockQuantity.Should().Be(8);
        }

        [Fact]
        public void IncreaseStock_Should_Throw_When_Quantity_Not_Positive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);

            // Act
            Action act = () => inv.IncreaseStock(0);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*greater than zero*");
        }

        [Fact]
        public void IncreaseStock_Should_Throw_When_Inactive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);
            inv.Deactivate();

            // Act
            Action act = () => inv.IncreaseStock(2);

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive product*");
        }

        [Fact]
        public void ReduceStock_Should_Subtract_Quantity_When_Enough_Stock()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 10);

            // Act
            inv.ReduceStock(4);

            // Assert
            inv.StockQuantity.Should().Be(6);
        }

        [Fact]
        public void ReduceStock_Should_Throw_When_Quantity_Not_Positive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 10);

            // Act
            Action act = () => inv.ReduceStock(0);

            // Assert
            act.Should().Throw<ValidationException>()
               .WithMessage("*greater than zero*");
        }

        [Fact]
        public void ReduceStock_Should_Throw_When_Inactive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 10);
            inv.Deactivate();

            // Act
            Action act = () => inv.ReduceStock(3);

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*inactive product*");
        }

        [Fact]
        public void ReduceStock_Should_Throw_When_Insufficient_Stock()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 3);

            // Act
            Action act = () => inv.ReduceStock(5);

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*Insufficient stock*");
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);

            // Act
            inv.Deactivate();

            // Assert
            inv.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_Should_Throw_When_Already_Inactive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);
            inv.Deactivate();

            // Act
            Action act = () => inv.Deactivate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already inactive*");
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True_When_Inactive()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);
            inv.Deactivate();

            // Act
            inv.Activate();

            // Assert
            inv.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Activate_Should_Throw_When_Already_Active()
        {
            // Arrange
            var inv = new ProductInventory(Guid.NewGuid(), 5);

            // Act
            Action act = () => inv.Activate();

            // Assert
            act.Should().Throw<BusinessRuleException>()
               .WithMessage("*already active*");
        }
    }
}

