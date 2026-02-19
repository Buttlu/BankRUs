using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface IBankAccountRepository
{
    void Add(BankAccount bankAccount);
    void UpdateBalance(BankAccount bankAccount);
    Task<BankAccount?> GetById(Guid bankAccountId, CancellationToken cancellationToken);
    Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId, CancellationToken cancellationToken);
    
}
