using BankRUs.Application.Pagination;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.Services;

public interface ITransactionService
{
    Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query);
    Task CreateTransaction(Transaction transaction);
}
