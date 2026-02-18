using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    ILogger<IdentityService> logger
) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<IdentityService> _logger = logger;

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        // TODO: skapa använda med Identity
        ApplicationUser user = new() {
            UserName = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            SocialSecurityNumber = request.SocialSecurityNumber.Trim(),
            Email = request.Email.Trim(),
            EmailConfirmed = true,
        };        

        IdentityResult result = await _userManager.CreateAsync(user, password: "Aa111!");
        if (!result.Succeeded) {
            _logger.LogWarning("Failed to create user account");
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new Exception($"Unable to create user: {errors}");
        }
        
        await _userManager.AddToRoleAsync(user, Roles.Customer);
        _logger.LogInformation("Created user with id {UserId}", user.Id);

        return new CreateUserResult(UserId: Guid.Parse(user.Id));
    }
}
