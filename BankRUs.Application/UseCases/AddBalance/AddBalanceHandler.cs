using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.AddBalance;

public class AddBalanceHandler(
    IBankAccountRepository bankAccountRepository,
    ITransactionRepository transactionRepository
)
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<AddBalanceResult> HandleAsync(AddBalanceCommand command)
    {
        var bankAccount = await _bankAccountRepository.GetById(command.BankAccountId) 
            ?? throw new ArgumentException("Invalid ");

        bankAccount.Deposit(amount: command.Amount);

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

        return new AddBalanceResult (
            TransactionId: transaction.Id,
            UserId: transaction.UserId,            
            Type: transaction.Type,
            Amount: transaction.Amount,
            Currency: transaction.Currency,
            Reference: transaction.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: transaction.BalanceAfter
        );
    }
}