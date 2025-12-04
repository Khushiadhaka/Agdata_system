using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RewardSystem_Application.Configuration;
using System.Text;

namespace RewardSystem_API.Extensions
{
    public static class AuthenticationExtensions
    {
        // Adds JWT authentication to DI container
        public static IServiceCollection AddJwtAuth(
            this IServiceCollection services, IConfiguration config)
        {
            // Read JwtSettings section and ensure it's NOT null
            var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>()
                              ?? throw new InvalidOperationException(
                                  "JwtSettings section is missing or invalid in appsettings.json");

            // Register strongly-typed settings as singleton
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });

            return services;
        }
    }
}
