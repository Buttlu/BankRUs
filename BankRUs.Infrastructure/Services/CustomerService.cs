using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Services;

public class CustomerService(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ILogger<CustomerService> logger
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CustomerService> _logger = logger;

    public async Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersQuery query)
    {
        var (customers, customerCount) = await _customerRepository.GetAllAsync(query);
        _logger.LogInformation("Found {CustomerCount} customers", customerCount);

        int totalPages = (int)Math.Ceiling((double)customerCount / query.PageSize);

        return new PagedResponse<CustomerDto>(
            Data: customers,
            MetaData: new PageMetaData(
                Page: query.Page,
                PageSize: query.PageSize,
                TotalCount: customerCount,
                TotalPages: totalPages
            )
        );
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var user = await _customerRepository.GetByIdAsync(id.ToString());
        
        if (user is null) {
            _logger.LogWarning("User with Id: {UserId} not found", id);
            return null;
        }
        _logger.LogInformation("Found user: {UserId}", user.CustomerId);

        return user;
    }

    public async Task UpdateCustomerInfo(Guid userId, UpdateUserDto updateDto)
    {
        await _customerRepository.UpdateUserAsync(userId, updateDto);
        _logger.LogInformation("Updated information of user: {UserId}", userId);
    }

    public async Task<bool> DeleteCustomer(Guid customerId)
    {
        var succeeded = await _customerRepository.DeleteAsync(customerId);
        if (succeeded) {
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted user: {UserId}", customerId);
        } else {
            _logger.LogWarning("Failed to delete user: {UserId}", customerId);
        }
        return succeeded;
    }
}
