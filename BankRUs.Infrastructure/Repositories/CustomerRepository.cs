using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersQuery query)
    {
        var customerQuery = _context.Users
            .AsNoTracking();
        
        var customers = await customerQuery.Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new CustomerDto(
                CustomerId: Guid.Parse(a.Id),
                FirstName: a.FirstName,
                LastName: a.LastName,
                Email: a.Email!,
                BankAccounts: null
            ))
            .ToListAsync();

        return (customers, customerQuery.Count());
    }
}