using BankRUs.Application.Dtos.Customer;
using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<bool> DeleteCustomer(Guid customerId);
    Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersFiltersDto filters);
    Task<CustomerDto?> GetByIdAsync(Guid Id);
    Task UpdateCustomerInfo(Guid userId, UpdateUserDto updateDto);
}

