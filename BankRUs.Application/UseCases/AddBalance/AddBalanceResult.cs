namespace BankRUs.Application.UseCases.AddBalance;

public sealed record AddBalanceResult(
    Guid TransactionId, 
    Guid UserId, 
    string Type, 
    decimal Amount,
    string Currency,
    string Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);