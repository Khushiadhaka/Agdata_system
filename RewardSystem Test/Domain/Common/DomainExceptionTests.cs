using FluentAssertions;
using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Test.Domain.Common
{
    // Concrete subclass for testing abstract DomainException
    public sealed class TestDomainException : DomainException
    {
        public TestDomainException(string message) : base(message) { }
    }

    public class DomainExceptionTests
    {
        [Fact]
        public void Constructor_Should_Set_Message()
        {
            var ex = new TestDomainException("Test error");

            ex.Message.Should().Be("Test error");
            ex.Should().BeAssignableTo<Exception>();
        }
    }
}
