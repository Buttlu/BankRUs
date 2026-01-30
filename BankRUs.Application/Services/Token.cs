namespace BankRUs.Application.Services;

public sealed record Token(
    string AccessToken,
    DateTime ExpiresAtUtc
);