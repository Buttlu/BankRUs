using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<IReadOnlyList<Customer>> GetAllUsers(int page, int pageSize)
    {
        var users = await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new Customer {
                Id = Guid.Parse(u.Id),
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                SocialSecurityNumber = u.SocialSecurityNumber,
                NumberOfBankAccounts = ,// Get from BankAccountRepository
                BankAccounts = // Get from BankAccountRepository
            })
            .ToListAsync();
    }        
}