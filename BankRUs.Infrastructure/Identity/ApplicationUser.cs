using Microsoft.AspNetCore.Identity;

namespace BankRUs.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string SocialSecurityNumber { get; set; }
    public bool IsDeleted { get; private set; } = false;

    public void Delete()
        => IsDeleted = true;

    public void Restore()
        => IsDeleted = false;
}
