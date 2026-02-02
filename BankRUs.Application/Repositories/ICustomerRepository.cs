using BankRUs.Application.Services;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetAllUsers(int page = 1, int pageSize = 10);
}