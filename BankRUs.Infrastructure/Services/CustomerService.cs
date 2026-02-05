using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Infrastructure.Authentication;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BankRUs.Infrastructure.Services;

public class CustomerService(
    UserManager<ApplicationUser> userManager,
    ICustomerRepository customerRepository,
    IBankAccountRepository bankAccountRepository
) : ICustomerService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;

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
        var user = await _userManager.FindByIdAsync(Id.ToString());
        
        if (user is null) 
            return null;

        var bankAccounts = await _bankAccountRepository.GetByUserId(Id);

        return new CustomerDto(
            CustomerId: Guid.Parse(user.Id),
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email!,
                SocialSecurityNumber: user.SocialSecurityNumber,
                BankAccounts: bankAccounts
        );
    }
}
