using BankRUs.Application.Authentication;
using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Infrastructure.Authentication;

public class AuthenticationService(UserManager<ApplicationUser> userManager) : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<AuthenticatedUser?> AuthenticateUser(string username, string password)
    {
        // Kontrollera att användaren finns
        var user = await _userManager.FindByEmailAsync(username);
        if (user is null)
            return null;
        
        if (!await _userManager.CheckPasswordAsync(user, password))
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new AuthenticatedUser(
            UserId: Guid.Parse(user.Id), 
            Username: user.UserName!,
            Email: user.Email!,
            Roles: roles
        );
    }
}
