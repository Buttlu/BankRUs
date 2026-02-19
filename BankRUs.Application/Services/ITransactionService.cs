using BankRUs.Application.Pagination;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.Services;

public interface ITransactionService
{
    Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query, CancellationToken cancellationToken);
    Task CreateTransaction(Transaction transaction, CancellationToken cancellationToken);
}
