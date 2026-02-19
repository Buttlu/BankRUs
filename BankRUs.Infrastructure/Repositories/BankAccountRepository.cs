using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class BankAccountRepository(
    ApplicationDbContext context
) : IBankAccountRepository
{
    private readonly ApplicationDbContext _context = context;

    public void Add(BankAccount bankAccount)
        => _context.BankAccounts.Add(bankAccount);

    public async Task<BankAccount?> GetById(Guid bankAccountId, CancellationToken cancellationToken)
        => await _context.BankAccounts
        .AsNoTracking()
        .FirstOrDefaultAsync(b=> b.Id == bankAccountId, cancellationToken);

    public async Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId, CancellationToken cancellationToken)
        => await _context.BankAccounts
        .AsNoTracking()
        .Where(b => b.UserId == userId.ToString())
        .ToListAsync(cancellationToken);

    public void UpdateBalance(BankAccount bankAccount)
        => _context.BankAccounts.Update(bankAccount);
}
