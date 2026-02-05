using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;
using BankRUs.Infrastructure.Identity;
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

        if (query.Ssn is not null)
            customerQuery = customerQuery.Where(c => c.SocialSecurityNumber.Contains(query.Ssn));
        if (query.Email is not null)
            customerQuery = customerQuery.Where(c => c.Email!.Contains(query.Email));

        var customers = await customerQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new CustomerDto(
                CustomerId: Guid.Parse(a.Id),
                FirstName: a.FirstName,
                LastName: a.LastName,
                Email: a.Email!,
                SocialSecurityNumber: a.SocialSecurityNumber,
                BankAccounts: null
            ))
            .ToListAsync();

        return (customers, customerQuery.Count());
    }

    public async Task<CustomerDto?> GetByIdAsync(string id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (user is null) 
            return null;

        return new CustomerDto(
            CustomerId: Guid.Parse(user.Id),
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email!,
            SocialSecurityNumber: user.SocialSecurityNumber, 
            BankAccounts: null
        );
    }              

    public async Task UpdateUserAsync(Guid userId, UpdateUserDto updateDto)
    {
        string userIdStr = userId.ToString();
        ApplicationUser user = await _context.Users
            .AsNoTracking()
            .FirstAsync(u => u.Id == userIdStr);
        
        user.FirstName = updateDto.FirstName!;
        user.LastName = updateDto.LastName!;
        user.Email = updateDto.Email!;
        user.SocialSecurityNumber = updateDto.SocialSecuritNumber!;

        _context.Users.Update(user);
    }
}