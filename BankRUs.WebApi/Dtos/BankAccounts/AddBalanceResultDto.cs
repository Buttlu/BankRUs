namespace BankRUs.WebApi.Dtos.BankAccounts;

public sealed record AddBalanceResultDto(
    Guid TransactionId,
    Guid UserId,
    string Type,
    decimal Amount,
    string Currency,
    string Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);