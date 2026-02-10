using BankRUs.Application.Repositories;
using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler(ITransactionService transactionService, IBankAccountRepository bankAccountRepository)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;

    public async Task<GetTransactionsResult> HandleAsync(GetTransactionsQuery query)
    {
        var pageResult = await _transactionService.GetTransactionsAsPageResultAsync(query);
        var bankAccount = await _bankAccountRepository.GetById(query.BankAccountId)
            ?? throw new ArgumentException("Bank Account not found");

        return new GetTransactionsResult(
            AccountId: query.BankAccountId,
            Currency: "SEK",
            Balance: bankAccount.Balance,
            Paging: pageResult.MetaData,
            Transactions: pageResult.Data
        );
    }
}
