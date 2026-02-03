using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.AddBalance;

public class AddBalanceHandler(IBankAccountRepository bankAccountRepository)
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;

    public async Task<AddBalanceResult> HandleAsync(AddBalanceCommand command)
    {
        var bankAccount = _bankAccountRepository.GetById(command.BankAccountId) 
            ?? throw new ArgumentException("Invalid ");

        bankAccount.Deposit(amount: command.Amount);

        var transaction = new Transaction {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(bankAccount.UserId),
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            Type = "Deposit",
            Currency = "SEK",
            Amount = command.Amount,
            BalanceAfter = bankAccount.Balance
};

        await _bankAccountRepository.CreateTransaction(transaction);

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