// Defines contract for generating JWT tokens.
using System;
using System.Collections.Generic;

namespace RewardSystem_Application.Interfaces.Security
{
    // JWT token generator interface (implemented in Infrastructure layer).
    public interface IJwtTokenGenerator
    {
        // Generate a JWT token for a user with optional extra claims.
        string GenerateToken(
            Guid userId,
            string email,
            IDictionary<string, string>? additionalClaims = null);
    }
}

