namespace BankRUs.WebApi.Dtos.BankAccounts;

public sealed record TransactionQuery(
    DateTime? From,
    DateTime? To,
    string? Type,
    int Page = 1,
    int PageSize = 20,
    string Desc = "desc"
);
