using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Services;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork,
    ILogger<TransactionService> logger 
) : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TransactionService> _logger = logger;

    public async Task CreateTransaction(Transaction transaction, CancellationToken cancellationToken)
    {
        await _transactionRepository.CreateTransaction(transaction);
        await _unitOfWork.SaveAsync(cancellationToken);

        if (transaction.Type == "Deposit") {
            _logger.LogInformation("Deposit-Transaction with Id {TransactionId} created", transaction.Id);
            _logger.LogDebug("Added {Amount} to {BankAccount}", transaction.Amount, transaction.AccountId);
        } else {
            _logger.LogInformation("Withdrawal-Transaction with Id {TransactionId} created", transaction.Id);
            _logger.LogDebug("Withdrew {Amount} from {BankAccount}", transaction.Amount, transaction.AccountId);
        }
    }

    public async Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var (transactions, totalCount) = await _transactionRepository.GetAll(query, cancellationToken);
        _logger.LogInformation("Found {TotalTransactions} transactions", totalCount);

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
