using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersQuery query);
    Task UpdateUserAsync(Guid userId, UpdateUserDto updateDto);
    Task<CustomerDto?> GetByIdAsync(string id);
    Task<bool> DeleteAsync(Guid customerId);
}
