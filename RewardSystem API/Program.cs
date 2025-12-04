using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using RewardSystem_API.Mappings;
using RewardSystem_API.Services;
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
using RewardSystem_Infrastructure.Infrastructure.Scripts;
using RewardSystem_Infrastructure.Infrastructure.Security;
using RewardSystem_Infrastructure.Persistence.Repositories;

using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//
// 1. Controllers + Swagger
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // basic doc info
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Reward System API",
        Version = "v1",
        Description = "Reward Points & Redemption API"
    });

    // Bearer token config for Swagger UI
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {your JWT token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

//
// 2. DbContext + UnitOfWork
//
builder.Services.AddDbContext<RewardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RewardDb")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//
// 3. Repositories
//
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();

builder.Services.AddScoped<IEventDefinitionRepository, EventDefinitionRepository>();
builder.Services.AddScoped<IEventInstanceRepository, EventInstanceRepository>();
builder.Services.AddScoped<IEventRewardRuleRepository, EventRewardRuleRepository>();

builder.Services.AddScoped<IRewardRepository, RewardRepository>();
builder.Services.AddScoped<IRewardPointsRepository, RewardPointsRepository>();
builder.Services.AddScoped<IRewardTransactionRepository, RewardTransactionRepository>();
builder.Services.AddScoped<IPointsTransactionRepository, PointsTransactionRepository>();

builder.Services.AddScoped<IRedemptionRequestRepository, RedemptionRequestRepository>();
builder.Services.AddScoped<IRedemptionRecordRepository, RedemptionRecordRepository>();
builder.Services.AddScoped<IRedemptionProcessRepository, RedemptionProcessRepository>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//
// 4. Application services
//
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<IEventDefinitionService, EventDefinitionService>();
builder.Services.AddScoped<IEventInstanceService, EventInstanceService>();
builder.Services.AddScoped<IEventRewardRuleService, EventRewardRuleService>();

builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IRewardPointsService, RewardPointsService>();
builder.Services.AddScoped<IRewardTransactionService, RewardTransactionService>();

builder.Services.AddScoped<IRedemptionRequestService, RedemptionRequestService>();
builder.Services.AddScoped<IRedemptionProcessService, RedemptionProcessService>();
builder.Services.AddScoped<IRedemptionRecordService, RedemptionRecordService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();

//
// 5. API-layer helper services
//
builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped<IProductApiService, ProductApiService>();
builder.Services.AddScoped<IInventoryApiService, InventoryApiService>();
builder.Services.AddScoped<IEventApiService, EventApiService>();
builder.Services.AddScoped<IRewardApiService, RewardApiService>();
builder.Services.AddScoped<IRedemptionApiService, RedemptionApiService>();

//
// 6. AutoMapper
//
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

//
// 7. Security: hashing + JWT
//
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value);

// Clear default claim type mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings configuration section is missing.");

var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();

//
// 8. Build + migrate + seed + run
//
var app = builder.Build();

// migrate + seed DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RewardDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reward System API v1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection(); // optional

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
