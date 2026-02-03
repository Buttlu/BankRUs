namespace BankRUs.WebApi.Dtos.BankAccounts;

public sealed record WithdrawBalanceResultDto(
    Guid TransactionId,
    Guid UserId,
    string Type,
    decimal Amount,
    string Currency,
    string Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);