using BankRUs.Application.Authentication.AuthenticateUser;

namespace BankRUs.Application.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticatedUser?> AuthenticateUser(string username, string password);
}
