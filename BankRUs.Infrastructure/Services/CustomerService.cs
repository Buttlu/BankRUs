using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetCustomers;

namespace BankRUs.Infrastructure.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<IReadOnlyList<Customer>> GetAll(int page, int pageSize)
        => await _customerRepository.GetAllUsers(page, pageSize);
}
