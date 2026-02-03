using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;
using BankRUs.Application.UseCases.GetCustomers;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersQuery query);
    Task<CustomerDto?> GetById(Guid Id);
}

