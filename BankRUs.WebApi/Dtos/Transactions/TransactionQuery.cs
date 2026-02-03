namespace BankRUs.WebApi.Dtos.Transactions;

public sealed record TransactionQuery(
    DateTime? From,
    DateTime? To,
    string? Type,
    int Page = 1,
    int PageSize = 20,
    string Sort = "desc"
);
