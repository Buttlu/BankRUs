using BankRUs.Application.Authentication;
using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.AddBalance;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Application.UseCases.WithdrawBalance;
using BankRUs.Infrastructure.Authentication;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using BankRUs.Infrastructure.Repositories;
using BankRUs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Handlers
builder.Services.AddScoped<OpenAccountHandler>();
builder.Services.AddScoped<AuthenticateUserHandler>();
builder.Services.AddScoped<OpenBankAccountHandler>();
builder.Services.AddScoped<AddBalanceHandler>();
builder.Services.AddScoped<WithdrawBalanceHandler>();
builder.Services.AddScoped<GetTransactionsHandler>();

// Repositories
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
if (builder.Environment.IsDevelopment()) {
    builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
} else {
    builder.Services.AddScoped<IEmailSender, EmailSender>();
}

// Settings from appsettings.json
builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection(PaginationOptions.SectionName));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

// Misc
builder.Services.AddScoped<IAccountNumberGenerator, AccountNumberGenerator>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration
        .GetSection(JwtOptions.SectionName)
        .Get<JwtOptions>()!;

    // false endast i dev
    options.RequireHttpsMetadata = builder.Environment.IsProduction();
    options.SaveToken = true;

    options.TokenValidationParameters = new() {
        ValidateIssuer = true,
        ValidIssuer = jwt.Issuer,

        ValidateAudience = true,
        ValidAudience = jwt.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),

        NameClaimType = JwtRegisteredClaimNames.Name,
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); 

    await new IdentitySeeder().SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

// This order matters Authen before author
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok("The API is running"));

//app.MapCustomersEndpoints();

app.Run();