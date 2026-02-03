using BankRUs.Application.Pagination;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.GetTransactions;

public sealed record GetTransactionsResult(
    Guid AccountId,
    string Currency,
    decimal Balance,
    PageMetaData Paging,
    IReadOnlyList<Transaction> Transactions
);
