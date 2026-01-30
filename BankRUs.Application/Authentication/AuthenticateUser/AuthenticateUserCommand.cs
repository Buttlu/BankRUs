namespace BankRUs.Application.Authentication.AuthenticateUser;

public sealed record AuthenticateUserCommand(
    string Username, 
    string Password
);
