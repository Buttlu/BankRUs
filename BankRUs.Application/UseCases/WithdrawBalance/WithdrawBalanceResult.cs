namespace BankRUs.Application.UseCases.WithdrawBalance;

public sealed record WithdrawBalanceResult(
    Guid TransactionId,
    Guid UserId,
    string Type,
    decimal Amount,
    string Currency,
    string Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);