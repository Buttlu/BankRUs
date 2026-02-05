using BankRUs.Application.Services;
using BankRUs.Domain.Entities;
using System.Collections.Generic;

namespace BankRUs.Application.UseCases.GetCustomers;

public class GetCustomersHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<GetCustomersResult> GetAllAsync(GetCustomersQuery query)
    {
        var pagedCustomers = await _customerService.GetAllAsync(query);

        return new GetCustomersResult(
            Customers: pagedCustomers.Data,
            PageMetaData: pagedCustomers.MetaData
        );
    }

    public async Task<GetCustomerByIdResult> GetByIdAsync(GetCustomerByIdQuery query)
    {
        var customer = await _customerService.GetByIdAsync(query.UserId)
            ?? throw new ArgumentException("Customer not found");

        return new GetCustomerByIdResult(
            UserId: customer.CustomerId,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            SocialSecurityNumber: customer.SocialSecurityNumber,
            BankAccounts: customer.BankAccounts ?? []
        );
    }
}
