using BankRUs.Application.Services;
using BankRUs.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Services;

public class UnitOfWork(
    ApplicationDbContext context,
    ILogger<UnitOfWork> logger
) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = logger;

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Saved changes to database");
    }
}
