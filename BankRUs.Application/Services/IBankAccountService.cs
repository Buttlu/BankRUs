using BankRUs.Domain.Entities;

namespace BankRUs.Application.Services;

public interface IBankAccountService
{
    Task Add(BankAccount bankAccount);
    Task<BankAccount?> GetById(Guid bankAccountId);
    Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId);
    Task UpdateBalance(BankAccount bankAccount);
}
