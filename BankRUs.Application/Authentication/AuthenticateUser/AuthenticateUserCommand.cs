namespace BankRUs.Application.Authentication;

public sealed record AuthenticateUserCommand(
    string Username, 
    string Password
);
