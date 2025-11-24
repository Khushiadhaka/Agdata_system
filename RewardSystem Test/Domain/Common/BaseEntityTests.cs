using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace RewardSystem_Test.Domain.Common
{
    // Simple test entity to access protected members
    public sealed class TestEntity : BaseEntity
    {
        public void Touch() => MarkUpdated();
    }

    public class BaseEntityTests
    {
        [Fact]
        public void Constructor_Should_Set_Id_And_CreatedAt()
        {
            // Act
            var entity = new TestEntity();

            // Assert
            entity.Id.Should().NotBe(Guid.Empty);
            entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            entity.UpdatedAt.Should().BeNull();
        }
     
        [Fact]
        public void MarkUpdated_Should_Set_UpdatedAt()
        {
            var entity = new TestEntity();

            entity.Touch();

            entity.UpdatedAt.Should().NotBeNull();
            entity.UpdatedAt.Should().BeAfter(entity.CreatedAt);
        }
    }
}
