using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;
using BankRUs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class CustomerRepository(
    UserManager<ApplicationUser> userManager
) : ICustomerRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersQuery query)
    {
        var customerQuery = _userManager.Users
            .Where(u => !u.IsDeleted);

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
        var user = await _userManager.FindByIdAsync(id);

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