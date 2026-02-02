using BankRUs.Application.DataObjects;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<Customer>> GetAll(int page, int pageSize);
}
