using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using Microsoft.Extensions.Logging;

namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler(
    ITransactionService transactionService, 
    IBankAccountRepository bankAccountRepository,
    ILogger<GetTransactionsHandler> logger
)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly ILogger<GetTransactionsHandler> _logger = logger;

    public async Task<GetTransactionsResult> HandleAsync(GetTransactionsQuery query)
    {
        var bankAccount = await _bankAccountRepository.GetById(query.BankAccountId);
        if (bankAccount is null) {
            _logger.LogWarning("Bank Account with Id: {BankAccountId} not found", query.BankAccountId);
            throw new ArgumentException("Bank Account not found");
        }
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
