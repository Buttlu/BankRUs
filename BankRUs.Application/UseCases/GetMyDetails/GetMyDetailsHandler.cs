using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.GetMyDetails;

public class GetMyDetailsHandler(
    ICustomerService customerService,
    IBankAccountService bankAccountService
)
{
    private readonly ICustomerService _customerService = customerService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<GetMyDetailsResult> HandleAsync(GetMyDetailsQuery query)
    {
        var customer = await _customerService.GetByIdAsync(query.UserId)
            ?? throw new ArgumentException("Customer not found");

        var bankAccounts = await _bankAccountService.GetByUserId(query.UserId);

        return new GetMyDetailsResult(
            Id: customer.CustomerId,
            Email: customer.Email,
            FullName: $"{customer.FirstName} {customer.LastName}",
            SocialSecurityNumber: customer.SocialSecurityNumber,
            BankAccounts: bankAccounts ?? []
        );
    }
}
