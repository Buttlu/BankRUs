namespace BankRUs.Application.UseCases.WithdrawBalance;

public sealed record WithdrawBalanceCommand(
    Guid BankAccountId,
    decimal Amount,
    string Reference
);
