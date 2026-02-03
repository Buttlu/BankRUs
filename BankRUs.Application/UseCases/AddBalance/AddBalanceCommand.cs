namespace BankRUs.Application.UseCases.AddBalance;

public sealed record AddBalanceCommand(
    Guid BankAccountId,
    decimal Amount, 
    string Reference
);
