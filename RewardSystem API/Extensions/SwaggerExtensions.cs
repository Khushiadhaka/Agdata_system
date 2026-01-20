using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RewardSystem_API.Extensions
{
	public static class SwaggerExtensions
	{
		public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "RewardSystem API",
					Version = "v1",
					Description = "AGDATA Reward Points System API"
				});

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter JWT token like: Bearer {token}"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app)
		{
			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "RewardSystem API v1");
				c.RoutePrefix = string.Empty;
			});

			return app;
		}
	}
}
