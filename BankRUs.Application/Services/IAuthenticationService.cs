using BankRUs.Application.Authentication.AuthenticateUser;

namespace BankRUs.Application.Services;

public interface IAuthenticationService
{
    Task<AuthenticatedUser?> AuthenticateUser(string username, string password);
}
