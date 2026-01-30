using BankRUs.Application.Services;

namespace BankRUs.Application.Authentication.AuthenticateUser;

public sealed class AuthenticateUserHandler(IAuthenticationService authenticationService, ITokenService tokenService)
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthenticateUserResult> HandleAsync(AuthenticateUserCommand command)
    {
        // Hitta användare
        var authenticatedUser = await _authenticationService.AuthenticateUser(
            username: command.Username,
            password: command.Password);

        // TODO: fel inloggningsuppgifter
        if (authenticatedUser is null) {
            return AuthenticateUserResult.Failed();
        }

        // Skapa JWT token
        var token = _tokenService.CreateToken(
            UserId: authenticatedUser.UserId.ToString(),
            Email: authenticatedUser.Email);
        
        // Returnera
        return AuthenticateUserResult.Succeeded(token.AccessToken, token.ExpiresAtUtc);
    }
}

