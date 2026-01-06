using RewardSystem_Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Security
{
    // Implementation of IPasswordHasher used for user credentials.
    public sealed class PasswordHasher : IPasswordHasher
    {
        // Generate a salted hash for a plain text password.
        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            // Generate random salt (16 bytes).
            var saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            // Compute hash.
            var hash = ComputeHash(password, salt);

            // Format: {salt}:{hash}
            return $"{salt}:{hash}";
        }

        // Verify a plain password against stored hash.
        public bool Verify(string hashedPassword, string inputPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(inputPassword))
                return false;

            // Split into salt and hash.
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                return false;

            var salt = parts[0];
            var expectedHash = parts[1];

            // Compute hash for provided password.
            var actualHash = ComputeHash(inputPassword, salt);

            return actualHash == expectedHash;
        }

        // Internal utility for hashing password + salt.
        private static string ComputeHash(string password, string salt)
        {
            using var sha = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = sha.ComputeHash(combined);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
