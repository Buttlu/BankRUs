using BankRUs.Application.Dtos.Customer;
using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<bool> DeleteCustomer(Guid customerId, CancellationToken cancellationToken);
    Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersFiltersDto filters, CancellationToken cancellationToken);
    Task<CustomerDto?> GetByIdAsync(Guid Id);
    Task UpdateCustomerInfo(Guid userId, UpdateUserDto updateDto);
}

