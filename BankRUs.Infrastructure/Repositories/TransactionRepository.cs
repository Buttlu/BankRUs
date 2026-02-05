using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class TransactionRepository(
    ApplicationDbContext context) : ITransactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task CreateTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetFromBankAccountId(Guid bankAccountId)
        => await _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == bankAccountId)
            .ToListAsync();

    public async Task<(IReadOnlyList<Transaction> transactions, int count)> GetAll(GetTransactionsQuery query)
    {
        var transactions = _context.Transactions.AsNoTracking();

        if (query.From is not null)
            transactions = transactions.Where(t => t.CreatedAt > query.From);

        if (query.To is not null)
            transactions = transactions.Where(t => t.CreatedAt < query.To);

        if (query.Type?.ToLower() == "deposit" || query.Type?.ToLower() == "withdrawal")
            transactions = transactions.Where(t => t.Type == query.Type);

        transactions = query.Desc switch {
            "asc" => transactions.OrderBy(t => t.CreatedAt),
            _ => transactions.OrderByDescending(t => t.CreatedAt),
        };

        var items = await transactions
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var count = transactions.Count();

        return (items, count);
    }
}
