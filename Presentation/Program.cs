using Application.BusinessRules;
using Application.BusinessRules.Interfaces;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Presentation.Middleware;
using Presentation.Seeders;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

#region MySql configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
#endregion

#region JWT configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
#endregion

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();


#region Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenInformationsServices, TokenInformationsServices>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

#region Repositories
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IGamePromotionRepository, GamePromotionRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region Rules
builder.Services.AddScoped<IUserRules, UserRules>();
#endregion

#region Mappers
builder.Services.AddAutoMapper(typeof(GameProfile));
builder.Services.AddAutoMapper(typeof(LibraryProfile));
builder.Services.AddAutoMapper(typeof(PromotionProfile));
builder.Services.AddAutoMapper(typeof(GamePromotionProfile));
builder.Services.AddAutoMapper(typeof(SaleProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(WalletProfile));
#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fiap Cloud Games API",
        Version = "v1",
        Description = "Documentação detalhada da API com códigos de retorno."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedAsync(services);
    await UserSeeder.SeedAsync(services);
    await GameSeeder.SeedAsync(services);
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
