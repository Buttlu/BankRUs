using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.WithdrawBalance;

public class WithdrawBalanceHandler(
    IBankAccountRepository bankAccountRepository,
    ITransactionRepository transactionRepository
)
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<WithdrawBalanceResult> HandleAsync(WithdrawBalanceCommand command)
    {
        var bankAccount = await _bankAccountRepository.GetById(command.BankAccountId)
            ?? throw new ArgumentException("Bank Account Not Found");
        try {
            bankAccount.Withdraw(command.Amount);
        } catch (ArgumentException ae) {
            throw new ArithmeticException(ae.Message);
        }

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
