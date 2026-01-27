using BankRUs.Application.Identity;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using BankRUs.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<OpenAccountHandler>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
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