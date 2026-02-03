using BankRUs.Application.Identity;

namespace BankRUs.Application.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetAll(int page = 1, int pageSize = 20);
    Task<CustomerDto?> GetById(Guid Id);
}

