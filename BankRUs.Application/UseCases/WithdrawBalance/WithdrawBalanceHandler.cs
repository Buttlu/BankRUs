using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.WithdrawBalance;

public class WithdrawBalanceHandler(
    ITransactionService transactionService,
    IBankAccountService bankAccountService
)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<WithdrawBalanceResult> HandleAsync(WithdrawBalanceCommand command)
    {
        var bankAccount = await _bankAccountService.GetById(command.BankAccountId)
            ?? throw new ArgumentException("Bank Account Not Found");

        bankAccount.Withdraw(command.Amount);
        await _bankAccountService.UpdateBalance(bankAccount);

        var transaction = new Transaction {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(bankAccount.UserId),
            AccountId = command.BankAccountId,
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            Type = "Withdrawal",
            Currency = "SEK",
            Amount = command.Amount,
            BalanceAfter = bankAccount.Balance
        };

        await _transactionService.CreateTransaction(transaction);

        return new WithdrawBalanceResult (
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
