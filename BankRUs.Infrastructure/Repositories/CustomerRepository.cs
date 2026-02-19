using BankRUs.Application.Dtos.Customer;
using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class CustomerRepository(
    UserManager<ApplicationUser> userManager
) : ICustomerRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersFiltersDto filters, CancellationToken cancellationToken)
    {
        var customerQuery = _userManager.Users
            .Where(u => !u.IsDeleted);

        if (filters.Ssn is not null)
            customerQuery = customerQuery.Where(c => c.SocialSecurityNumber.Contains(filters.Ssn));
        if (filters.Email is not null)
            customerQuery = customerQuery.Where(c => c.Email!.Contains(filters.Email));

        var customers = await customerQuery
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Select(a => new CustomerDto(
                CustomerId: Guid.Parse(a.Id),
                FirstName: a.FirstName,
                LastName: a.LastName,
                Email: a.Email!,
                SocialSecurityNumber: a.SocialSecurityNumber,
                BankAccounts: null
            ))
            .ToListAsync(cancellationToken);

        var customerCount = await customerQuery.CountAsync(cancellationToken);

        return (customers, customerCount);
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null || user.IsDeleted) 
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
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null || !user.IsDeleted)
            throw new ArgumentException("User could not be found");

        user.FirstName = updateDto.FirstName!;
        user.LastName = updateDto.LastName!;
        user.Email = updateDto.Email!;
        user.SocialSecurityNumber = updateDto.SocialSecuritNumber!;

        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null || user.IsDeleted) return false;

        user.Delete();

        return true;
    }
}