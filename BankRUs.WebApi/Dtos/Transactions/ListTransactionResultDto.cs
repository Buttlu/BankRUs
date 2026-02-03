using BankRUs.Application.Pagination;
using BankRUs.Domain.Entities;

namespace BankRUs.WebApi.Dtos.Transactions;

public sealed record ListTransactionResultDto(
    Guid AccountId,
    string Currency,
    decimal Balance,
    PageMetaData Paging,
    IReadOnlyList<Transaction> Items
);
