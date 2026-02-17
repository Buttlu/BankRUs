using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.AddBalance;

public class AddBalanceHandler(ITransactionService transactionService)
{
    private readonly ITransactionService _transactionService = transactionService;

    public async Task<AddBalanceResult> HandleAsync(AddBalanceCommand command)
    {
        var transaction = await _transactionService.AddBalance(command);
        
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