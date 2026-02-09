using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<bool> DeleteCustomer(Guid customerId);
    Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersQuery query);
    Task<CustomerDto?> GetByIdAsync(Guid Id);
    Task UpdateCustomerInfo(Guid userId, UpdateUserDto updateDto);
}

