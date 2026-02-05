using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankRUs.Infrastructure.Repositories;

public class BankAccountRepository(ApplicationDbContext context) : IBankAccountRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
    }    

    public BankAccount? GetById(Guid bankAccountId)
        => _context.BankAccounts.Find(bankAccountId);

    public async Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId)
        => await _context.BankAccounts
        .Where(b => b.UserId == userId.ToString())
        .ToListAsync();
}
