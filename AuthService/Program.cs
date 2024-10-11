using AuthService.Services;
using DbContextDao;
using JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Models;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var accessJwtConfig = builder.Configuration.GetSection("AccessJwtConfig").Get<JwtConfig>();
var refreshJwtConfig = builder.Configuration.GetSection("RefreshJwtConfig").Get<JwtConfig>();

builder.Services.AddScoped<IAccessTokenService>(t => new TokenService(accessJwtConfig));
builder.Services.AddScoped<IRefreshTokenService>(t => new TokenService(refreshJwtConfig));
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(accessJwtConfig.SigningKeyBytes),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,

            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudiences = new[] { accessJwtConfig.Audience },
            ValidIssuer = accessJwtConfig.Issuer
        };
    });
builder.Services.AddAuthorization();

/*var dbConfig = builder.Configuration.GetSection("MySqlConfig");

builder.Services.AddDbContext<AuthContext>(
    options => options.UseMySql(
                 $@"server={dbConfig["Server"]};
                   port={dbConfig["Port"]};
                   user={dbConfig["User"]};
                   password={dbConfig["Password"]};
                   database={dbConfig["Database"]};",
                 new MySqlServerVersion(dbConfig["Version"]))
    );*/

var dbConfig = builder.Configuration.GetSection("SqLiteConfig");
builder.Services.AddDbContext<AuthContext>(
    options => options.UseSqlite($"Data Source={dbConfig["Filename"]};")
    );

 builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Policy1",
        policy =>
        {
            policy
            //.WithOrigins("https://localhost:7256/")
            //.WithHeaders(HeaderNames.Authorization, "*")
            //.WithMethods("POST", "GET");
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("Policy1");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
