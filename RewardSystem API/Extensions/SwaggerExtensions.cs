using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;


namespace RewardSystem_API.Extensions
{
    // Extension methods to configure Swagger + JWT support.
    public static class SwaggerExtensions
    {
        // Register Swagger generator and basic JWT bearer scheme.
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            // Discover minimal API endpoints.
            services.AddEndpointsApiExplorer();

            // Register Swagger generator.
            services.AddSwaggerGen(c =>
            {
                // Basic doc info.
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RewardSystem API",
                    Version = "v1",
                    Description = "AGDATA Reward Points System API"
                });

                // Define JWT bearer authentication scheme (for the Authorize button).
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",              // header name
                    Type = SecuritySchemeType.Http,      // HTTP auth scheme
                    Scheme = "bearer",                   // must be "bearer"
                    BearerFormat = "JWT",                // token format
                    In = ParameterLocation.Header,       // in request header
                    Description = "Enter JWT token in the format: Bearer {token}"
                });

                
            });

            return services;
        }

        // Add Swagger middlewares + UI.
        public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app)
        {
            // Generate Swagger JSON.
            app.UseSwagger();

            // Serve UI.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RewardSystem API v1");
                c.RoutePrefix = string.Empty; // Swagger UI at root /
            });

            return app;
        }
    }
}
