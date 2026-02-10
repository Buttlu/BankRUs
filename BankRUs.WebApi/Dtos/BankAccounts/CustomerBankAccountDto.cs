namespace BankRUs.WebApi.Dtos.BankAccounts;

public sealed record CustomerBankAccountDto(
    Guid Id,
    string Name,
    string AccountNumber,
    decimal Balance
);