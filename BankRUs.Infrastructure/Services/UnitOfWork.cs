using BankRUs.Application.Services;
using BankRUs.Infrastructure.Persistance;

namespace BankRUs.Infrastructure.Services;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
