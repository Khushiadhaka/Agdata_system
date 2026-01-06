using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RewardSystem_API.Mappings;
using RewardSystem_Application.Common;
using RewardSystem_Application.Configuration;
using RewardSystem_Application.Interfaces.Auth;
using RewardSystem_Application.Interfaces.Event;
using RewardSystem_Application.Interfaces.Inventory;
using RewardSystem_Application.Interfaces.Product;
using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Interfaces.Reward;
using RewardSystem_Application.Interfaces.Security;
using RewardSystem_Application.Interfaces.Transaction;
using RewardSystem_Application.Interfaces.Users;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services;
using RewardSystem_Infrastructure.Infrastructure.Authentication;
using RewardSystem_Infrastructure.Infrastructure.Persistence;
using RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories;
using RewardSystem_Infrastructure.Infrastructure.Security;
using RewardSystem_Infrastructure.Persistence.Repositories;

namespace RewardSystem_API.Extensions
{
    /// <summary>
    /// Helper methods to register all project layers into DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// High-level helper: register Application + Infrastructure + JWT.
        /// Call this from Program.cs.
        /// </summary>
        public static IServiceCollection AddRewardSystemCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationLayer();                 // application services + AutoMapper
            services.AddInfrastructureLayer(configuration); // DbContext + repositories + infra services
            services.AddJwtAuth(configuration);             // JWT auth (AuthenticationExtensions)

            return services;
        }

        /// <summary>
        /// Register application-layer services & AutoMapper.
        /// </summary>
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services)
        {
            // AutoMapper profile for DTO <-> Domain mappings.
            
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Auth + user services.
            services.AddScoped<IAuthService, AuthService>();                      // login/register
            services.AddScoped<IUserService, UserService>();                      // manage users
            services.AddScoped<IUserProfileService, UserProfileService>();        // profiles
            services.AddScoped<IUserAccountService, UserAccountService>();        // points accounts

            // Product / inventory services.
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInventoryService, InventoryService>();

            // Event services.
            services.AddScoped<IEventDefinitionService, EventDefinitionService>();
            services.AddScoped<IEventInstanceService, EventInstanceService>();
            services.AddScoped<IEventRewardRuleService, EventRewardRuleService>();

            // Reward services.
            services.AddScoped<IRewardService, RewardService>();
            services.AddScoped<IRewardPointsService, RewardPointsService>();
            services.AddScoped<IRewardTransactionService, RewardTransactionService>();

            // Redemption services.
            services.AddScoped<IRedemptionRequestService, RedemptionRequestService>();
            services.AddScoped<IRedemptionProcessService, RedemptionProcessService>();
            services.AddScoped<IRedemptionRecordService, RedemptionRecordService>();

            // Business transactions service.
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }

        /// <summary>
        /// Register infrastructure-layer services: DbContext, UnitOfWork, repositories, security.
        /// </summary>
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ---------- DbContext + UoW ----------

            // Read connection string from appsettings.
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException(
                                      "ConnectionStrings:DefaultConnection is missing in appsettings.json");

            // EF Core DbContext.
            services.AddDbContext<RewardDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Unit of work.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ---------- Security / JWT ----------

            // Strongly typed JwtSettings.
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                              ?? throw new InvalidOperationException(
                                  "JwtSettings section is missing or invalid in appsettings.json");

            services.AddSingleton(jwtSettings);                         // config object
            services.AddScoped<IPasswordHasher, PasswordHasher>();      // password hashing
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(); // JWT generator

            // ---------- Repositories (EF implementations) ----------

            // User repositories.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            // Product repositories.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();

            // Event repositories.
            services.AddScoped<IEventDefinitionRepository, EventDefinitionRepository>();
            services.AddScoped<IEventInstanceRepository, EventInstanceRepository>();
            services.AddScoped<IEventRewardRuleRepository, EventRewardRuleRepository>();

            // Reward repositories.
            services.AddScoped<IRewardRepository, RewardRepository>();
            services.AddScoped<IRewardPointsRepository, RewardPointsRepository>();
            services.AddScoped<IRewardTransactionRepository, RewardTransactionRepository>();
            services.AddScoped<IPointsTransactionRepository, PointsTransactionRepository>();

            // Redemption repositories.
            services.AddScoped<IRedemptionRequestRepository, RedemptionRequestRepository>();
            services.AddScoped<IRedemptionRecordRepository, RedemptionRecordRepository>();
            services.AddScoped<IRedemptionProcessRepository, RedemptionProcessRepository>();

            // Transaction repository.
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
