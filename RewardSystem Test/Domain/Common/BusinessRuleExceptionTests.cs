using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace RewardSystem_Test.Domain.Common
{
    public class BusinessRuleExceptionTests
    {
        [Fact]
        public void BusinessRuleException_Should_Inherit_DomainException()
        {
            var ex = new BusinessRuleException("Rule broken");

            ex.Should().BeAssignableTo<DomainException>();
            ex.Message.Should().Be("Rule broken");
        }
    }
}
