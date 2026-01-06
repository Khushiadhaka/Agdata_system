using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Security
{
    // Simple password hashing/verification abstraction used by AuthService.
    public interface IPasswordHasher
    {
        // Produce a stored password hash from a plain password.
        string Hash(string plainPassword);

        // Verify plain password against stored hash.
        bool Verify(string storedHash, string plainPassword);
    }
}
