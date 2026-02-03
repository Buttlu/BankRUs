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
    ICustomerRepository customerRepository
) : ICustomerService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ICustomerRepository _customerRepository = customerRepository;

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

    public async Task<CustomerDto?> GetById(Guid Id)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());
        
        if (user is null) 
            return null;
        
        return new CustomerDto(
            CustomerId: Guid.Parse(user.Id),
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email!,
                BankAccounts: null
        );
    }
}
