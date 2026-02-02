namespace BankRUs.Application.DataObjects;

public sealed record Token(
    string AccessToken,
    DateTime ExpiresAtUtc
);