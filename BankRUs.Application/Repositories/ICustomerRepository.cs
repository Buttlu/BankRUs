using BankRUs.Application.Dtos.Customer;
using BankRUs.Application.Identity;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersFiltersDto filters);
    Task UpdateUserAsync(Guid userId, UpdateUserDto updateDto);
    Task<CustomerDto?> GetByIdAsync(string id);
    Task<bool> DeleteAsync(Guid customerId);
}
