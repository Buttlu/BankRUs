using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.Application.Services;
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

        return new AuthenticatedUser(
            Guid.Parse(user.Id), 
            user.UserName!,
            user.Email!
        );
    }
}
