using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using BankRUs.Infrastructure.Repositories;
using BankRUs.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.OpenBankAccount;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<OpenAccountHandler>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<OpenBankAccountHandler>();
if (builder.Environment.IsDevelopment()) {
    builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
} else {
    builder.Services.AddScoped<IEmailSender, EmailSender>();
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); 

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok("The API is running"));

//app.MapCustomersEndpoints();

app.Run();