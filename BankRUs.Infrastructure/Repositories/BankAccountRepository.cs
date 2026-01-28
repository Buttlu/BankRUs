using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;

namespace BankRUs.Infrastructure.Repositories;

public class BankAccountRepository(ApplicationDbContext context) : IBankAccountRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
    }
}
