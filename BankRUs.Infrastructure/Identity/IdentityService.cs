using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        // TODO: skapa använda med Identity
        ApplicationUser user = new() {
            UserName = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            SocialSecurityNumber = request.SocialSecurityNumber.Trim(),
            Email = request.Email.Trim(),
        };        

        IdentityResult result = await _userManager.CreateAsync(user, password: "Aa111!");
        if (!result.Succeeded) {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new Exception($"Unable to create user: {errors}");
        }

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        return new CreateUserResult(UserId: Guid.Parse(user.Id));
    }
}
