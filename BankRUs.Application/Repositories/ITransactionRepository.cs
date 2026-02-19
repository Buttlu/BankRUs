using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface ITransactionRepository
{
    Task CreateTransaction(Transaction transaction);
    Task<IReadOnlyList<Transaction>> GetFromBankAccountId(Guid bankAccountId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Transaction> transactions, int count)> GetAll(GetTransactionsQuery query, CancellationToken cancellationToken);
}
