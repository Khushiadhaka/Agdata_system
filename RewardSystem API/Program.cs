using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
using RewardSystem_Infrastructure.Infrastructure.Scripts;
using RewardSystem_Infrastructure.Infrastructure.Security;
using RewardSystem_Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Allow frontend (localhost:4200) to call this API during development
var frontendOrigin = "http://localhost:4200";
var corsPolicyName = "AllowFrontend";

//
// 1. CONFIGURATION
//
var connectionString = builder.Configuration.GetConnectionString("RewardDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
	throw new InvalidOperationException(
		"Connection string 'RewardDb' is missing. Check appsettings.json");
}

//
// 2. CONTROLLERS + SWAGGER
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: corsPolicyName,
		policy =>
		{
			policy.WithOrigins(frontendOrigin)
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Reward System API",
		Version = "v1",
		Description = "Reward Points & Redemption API"
	});

	var jwtScheme = new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "Enter: Bearer {JWT token}",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT"
	};

	c.AddSecurityDefinition("Bearer", jwtScheme);
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ jwtScheme, Array.Empty<string>() }
	});
});

//
// 3. DATABASE (EF CORE)
//
builder.Services.AddDbContext<RewardDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//
// 4. REPOSITORIES
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
builder.Services.AddScoped<IRedemptionProcessRepository, RedemptionProcessRepository>();
builder.Services.AddScoped<IRedemptionRecordRepository, RedemptionRecordRepository>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//
// 5. APPLICATION SERVICES (🔥 FIXED)
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
// 6. AUTOMAPPER
//
builder.Services.AddAutoMapper(cfg =>
{
	cfg.AddProfile<MappingProfile>();
});

//
// 7. JWT AUTHENTICATION
//
builder.Services.Configure<JwtSettings>(
	builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton(sp =>
	sp.GetRequiredService<IOptions<JwtSettings>>().Value);

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var jwtSettings = builder.Configuration
	.GetSection("JwtSettings")
	.Get<JwtSettings>()
	?? throw new InvalidOperationException("JwtSettings missing");

var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});

builder.Services.AddAuthorization();

//
// 8. BUILD APP
//
var app = builder.Build();

//
// 9. MIGRATE + SEED DATABASE
//
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<RewardDbContext>();
	await db.Database.MigrateAsync();
	await DbSeeder.SeedAsync(db);
}

//
// 10. MIDDLEWARE PIPELINE
//
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();

// Enable CORS for frontend
app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
