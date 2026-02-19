using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.AddBalance;

public class AddBalanceHandler(
    ITransactionService transactionService,
    IBankAccountService bankAccountService
)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<AddBalanceResult> HandleAsync(AddBalanceCommand command, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountService.GetById(command.BankAccountId, cancellationToken)
            ?? throw new ArgumentException("Bank Account not found");

        bankAccount.Deposit(amount: command.Amount);
        await _bankAccountService.UpdateBalance(bankAccount, cancellationToken);

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

        await _transactionService.CreateTransaction(transaction, cancellationToken);
        
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