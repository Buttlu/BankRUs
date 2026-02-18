using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using Microsoft.Extensions.Logging;

namespace BankRUs.Application.UseCases.GetCustomers;

public class GetCustomersHandler(
    ICustomerService customerService,
    IBankAccountService bankAccountService,
    ILogger<GetCustomersHandler> logger
)
{
    private readonly ICustomerService _customerService = customerService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;
    private readonly ILogger<GetCustomersHandler> _logger = logger;

    public async Task<GetCustomersResult> GetAllAsync(GetCustomersQuery query)
    {
        var pagedCustomers = await _customerService.GetAllAsync(new GetCustomersFiltersDto(
            Page: query.Page,
            PageSize: query.PageSize,
            Ssn: query.Ssn,
            Email: query.Email
        ));

        return new GetCustomersResult(
            Customers: pagedCustomers.Data,
            PageMetaData: pagedCustomers.MetaData
        );
    }

    public async Task<GetCustomerByIdResult> GetByIdAsync(GetCustomerByIdQuery query)
    {
        var customer = await _customerService.GetByIdAsync(query.UserId)
            ?? throw new ArgumentException("Customer not found");

        var bankAccounts = await _bankAccountService.GetByUserId(customer.CustomerId);
        _logger.LogInformation("Found {BankAccountCount} bank accounts belonging to {UserId}", bankAccounts.Count, customer.CustomerId);

        return new GetCustomerByIdResult(
            UserId: customer.CustomerId,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            SocialSecurityNumber: customer.SocialSecurityNumber,
            BankAccounts: bankAccounts ?? []
        );
    }
}
