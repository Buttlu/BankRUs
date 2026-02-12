using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.WithdrawBalance;

public class WithdrawBalanceHandler(ITransactionService transactionService)
{
    private readonly ITransactionService _transactionService = transactionService;

    public async Task<WithdrawBalanceResult> HandleAsync(WithdrawBalanceCommand command)
    {
        var transaction = await _transactionService.WithdrawBalance(command);

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
