namespace BankRUs.Application.Authentication;

public sealed record Token(
    string AccessToken,
    DateTime ExpiresAtUtc
);