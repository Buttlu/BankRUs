using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount> CreateAsync(BankAccount bankAccount);
}
