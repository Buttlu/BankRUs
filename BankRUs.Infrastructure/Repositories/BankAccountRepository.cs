using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class BankAccountRepository(ApplicationDbContext context) : IBankAccountRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
    }    

    public async Task<BankAccount?> GetById(Guid bankAccountId)
        => await _context.BankAccounts.AsNoTracking().FirstOrDefaultAsync(b=> b.Id == bankAccountId);

    public async Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId)
        => await _context.BankAccounts
        .AsNoTracking()
        .Where(b => b.UserId == userId.ToString())
        .ToListAsync();
}
