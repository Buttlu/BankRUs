using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.GetMyDetails;

public class GetMyDetailsHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<GetMyDetailsResult> HandleAsync(GetMyDetailsQuery query)
    {
        var customer = await _customerService.GetByIdAsync(query.UserId)
            ?? throw new ArgumentException("Customer not found");

        return new GetMyDetailsResult(
            Id: customer.CustomerId,
            Email: customer.Email,
            FullName: $"{customer.FirstName} {customer.LastName}",
            SocialSecurityNumber: customer.SocialSecurityNumber,
            BankAccounts: customer.BankAccounts ?? []
        );
    }
}
