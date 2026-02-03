using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BankRUs.Infrastructure.Identity;

public class IdentitySeeder
{
    private RoleManager<IdentityRole> _roleManager = default!;
    private UserManager<ApplicationUser> _userManager = default!;

    public async Task SeedAsync(IServiceProvider services)
    {
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        
        await SeedRolesAsync();

        if (await _userManager.FindByEmailAsync("admin@bankrus.se") is null) {
            var user = await CreateUserAsync("admin@bankrus.se", "Admin", "Adminsson", "19800101-1111", "Aa111!");
            await _userManager.AddToRoleAsync(user, Roles.CustomerService);
            //await _userManager.AddToRoleAsync(user, Roles.Customer);
            await SeedUsers(200);
        }

    }

    private async Task SeedUsers(int amount)
    {
        var faker = new Faker("sv");
        for (int i = 0; i < amount; i++) {
            var result = await CreateUserAsync(
                fName: faker.Name.FirstName(),
                lName: faker.Name.LastName(),
                socialSecurityNumber: faker.Random.Replace("########-####"),
                email: faker.Internet.Email(),
                password: "Aa111!"
            );
        }
    }

    private async Task<ApplicationUser> CreateUserAsync(string email, string fName, string lName, string socialSecurityNumber, string password)
    {
        ApplicationUser? found = await _userManager.FindByEmailAsync(email);
        if (found is not null) return null!;

        ApplicationUser user = new() {
            UserName = email,
            Email = email,
            FirstName = fName,
            LastName = lName,
            EmailConfirmed = true,
            SocialSecurityNumber = socialSecurityNumber,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        return user;
    }

    private async Task SeedRolesAsync()
    {
        foreach (var role in Roles.All) {
            if (!await _roleManager.RoleExistsAsync(role)) {
                var result = await _roleManager.CreateAsync(new IdentityRole {
                    Name = role
                });
                if (!result.Succeeded) {
                    throw new Exception($"failed to create role '{role}', {result.Errors}");
                }
            }
        }
    }
}
