using BankRUs.Application.Pagination;
using BankRUs.Application.UseCases.AddBalance;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Application.UseCases.WithdrawBalance;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.Services;

public interface ITransactionService
{
    Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query);
    Task<Transaction> AddBalance(AddBalanceCommand command);
    Task<Transaction> WithdrawBalance(WithdrawBalanceCommand command);
}
