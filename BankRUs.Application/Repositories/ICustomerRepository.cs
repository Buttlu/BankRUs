using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.GetCustomers;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<CustomerDto>, int)> GetAllAsync(GetCustomersQuery query);
}
