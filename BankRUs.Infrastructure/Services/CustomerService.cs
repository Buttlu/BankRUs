using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;

namespace BankRUs.Infrastructure.Services;

public class CustomerService(
    ICustomerRepository customerRepository,
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;    

    public async Task<PagedResponse<CustomerDto>> GetAllAsync(GetCustomersQuery query)
    {
        var (customers, customerCount) = await _customerRepository.GetAllAsync(query);
        
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

    public async Task<CustomerDto?> GetByIdAsync(Guid Id)
    {
        var user = await _customerRepository.GetByIdAsync(Id.ToString());
        
        if (user is null) 
            return null;

        var bankAccounts = await _bankAccountRepository.GetByUserId(Id);

        return new CustomerDto(
            CustomerId: user.CustomerId,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email!,
                SocialSecurityNumber: user.SocialSecurityNumber,
                BankAccounts: bankAccounts
        );
    }

    public async Task UpdateCustomerInfo(Guid userId, UpdateUserDto updateDto)
    {
        await _customerRepository.UpdateUserAsync(userId, updateDto);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteCustomer(Guid customerId)
    {
        var succeeded = await _customerRepository.DeleteAsync(customerId);
        if (succeeded) {
            await _unitOfWork.SaveAsync();
        }
        return succeeded;
    }
}
