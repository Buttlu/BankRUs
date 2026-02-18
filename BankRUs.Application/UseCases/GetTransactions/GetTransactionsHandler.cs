using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using Microsoft.Extensions.Logging;

namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler(
    ITransactionService transactionService, 
    IBankAccountService bankAccountService
)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<GetTransactionsResult> HandleAsync(GetTransactionsQuery query)
    {
        var bankAccount = await _bankAccountService.GetById(query.BankAccountId)
            ?? throw new ArgumentException("Bank Account not found");

        var pageResult = await _transactionService.GetTransactionsAsPageResultAsync(query);

        return new GetTransactionsResult(
            AccountId: query.BankAccountId,
            Currency: "SEK",
            Balance: bankAccount.Balance,
            Paging: pageResult.MetaData,
            Transactions: pageResult.Data
        );
    }
}
