using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;

namespace BankRUs.Infrastructure.Services;

public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query)
    {
        var (transactions, totalCount) = await _transactionRepository.GetAll(query);

        int totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new PagedResponse<Transaction>(
            Data: transactions,
            new PageMetaData(
                Page: query.Page,
                PageSize: query.PageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            )
        );
    }
}
