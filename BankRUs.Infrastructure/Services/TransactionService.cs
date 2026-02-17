using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.AddBalance;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Application.UseCases.WithdrawBalance;
using BankRUs.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Services;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork,
    ILogger<TransactionService> logger 
) : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TransactionService> _logger = logger;

    public async Task<Transaction> AddBalance(AddBalanceCommand command)
    {
        var bankAccount = await _bankAccountRepository.GetById(command.BankAccountId);
        if (bankAccount is null) {
            _logger.LogWarning("Bank Account with Id: {BankAccountId} nout found", command.BankAccountId);
            throw new ArgumentException("Bank Account not found");
        }

        bankAccount.Deposit(amount: command.Amount);
        _bankAccountRepository.UpdateBalance(bankAccount);

        var transaction = new Transaction {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(bankAccount.UserId),
            AccountId = command.BankAccountId,
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            Type = "Deposit",
            Currency = "SEK",
            Amount = command.Amount,
            BalanceAfter = bankAccount.Balance
        };

        await _transactionRepository.CreateTransaction(transaction);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Deposit-Transaction with Id {TransactionId} created", transaction.Id);
        _logger.LogDebug("Added {Amount} to {BankAccount}", transaction.Amount, transaction.AccountId);

        return transaction;
    }

    public async Task<PagedResponse<Transaction>> GetTransactionsAsPageResultAsync(GetTransactionsQuery query)
    {
        var (transactions, totalCount) = await _transactionRepository.GetAll(query);
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

    public async Task<Transaction> WithdrawBalance(WithdrawBalanceCommand command)
    {
        var bankAccount = await _bankAccountRepository.GetById(command.BankAccountId)
            ?? throw new ArgumentException("Bank Account Not Found");
        
        bankAccount.Withdraw(command.Amount);
        _bankAccountRepository.UpdateBalance(bankAccount);

        var transaction = new Transaction {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(bankAccount.UserId),
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            Type = "Withdrawal",
            Currency = "SEK",
            Amount = command.Amount,
            BalanceAfter = bankAccount.Balance
        };

        await _transactionRepository.CreateTransaction(transaction);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Withdrawal-Transaction with Id \"{TransactionId}\" created", transaction.Id);
        _logger.LogDebug("Withdrew {Amount} from {BankAccount}", transaction.Amount, transaction.AccountId);

        return transaction;
    }
}
