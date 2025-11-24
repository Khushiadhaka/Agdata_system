using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace RewardSystem_Test.Domain.Common
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void ValidationException_Should_Inherit_DomainException()
        {
            var ex = new ValidationException("Invalid");

            ex.Should().BeAssignableTo<DomainException>();
            ex.Message.Should().Be("Invalid");
        }
    }
}
