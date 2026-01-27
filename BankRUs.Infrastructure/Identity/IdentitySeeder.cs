using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BankRUs.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        // Seeda data
        //  - roller

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedRolesAsync(roleManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles.All) {
            if (!await roleManager.RoleExistsAsync(role)) {
                var result = await roleManager.CreateAsync(new IdentityRole {
                    Name = role
                });
                if (!result.Succeeded) {
                    throw new Exception($"failed to create role '{role}', {result.Errors}");
                }
            }
        }
    }
}
