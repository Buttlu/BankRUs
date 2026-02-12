namespace BankRUs.Application.UseCases.GetTransactions;

public sealed record GetTransactionsQuery(
    Guid BankAccountId,

    DateTime? From,
    DateTime? To,
    string? Type,
    int Page = 1,
    int PageSize = 20,
    string Desc = "desc"
);