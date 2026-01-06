using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RewardSystem_Application.Configuration;
using RewardSystem_Application.Interfaces.Security;

namespace RewardSystem_Infrastructure.Infrastructure.Authentication
{
    // Implementation of IJwtTokenGenerator using JwtSecurityTokenHandler.
    public sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        // App JWT configuration settings (Issuer, Audience, Secret, ExpiryMinutes).
        private readonly JwtSettings _settings;

        // Inject JwtSettings via constructor.
        public JwtTokenGenerator(JwtSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        // Generate JWT token with optional additional claims.
        public string GenerateToken(
            Guid userId,
            string email,
            IDictionary<string, string>? additionalClaims = null)
        {
            // Secret key.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            // Signing credentials.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Base claims.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email)
            };

            // Add custom claims if provided.
            if (additionalClaims is not null)
            {
                foreach (var c in additionalClaims)
                {
                    claims.Add(new Claim(c.Key, c.Value));
                }
            }

            // Create JWT token.
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: creds);

            // Serialize token string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
