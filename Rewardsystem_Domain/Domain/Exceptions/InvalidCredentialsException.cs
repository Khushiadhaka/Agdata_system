using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Exceptions
{
    // Exception thrown when login credentials are invalid.
    public sealed class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() { }
        public InvalidCredentialsException(string message) : base(message) { }
        public InvalidCredentialsException(string message, Exception inner) : base(message, inner) { }
    }
}
