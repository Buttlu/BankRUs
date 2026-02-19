using BankRUs.Domain.Entities;

namespace BankRUs.Application.Services;

public interface IBankAccountService
{
    Task Add(BankAccount bankAccount, CancellationToken cancellationToken);
    Task<BankAccount?> GetById(Guid bankAccountId, CancellationToken cancellationToken);
    Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId, CancellationToken cancellationToken);
    Task UpdateBalance(BankAccount bankAccount, CancellationToken cancellationToken);
}
