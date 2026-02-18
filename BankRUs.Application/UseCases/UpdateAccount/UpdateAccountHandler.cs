using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.UpdateAccount;

public class UpdateAccountHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<UpdateAccountResult> HandleAsync(UpdateAccountCommand command)
    {
        var user = await _customerService.GetByIdAsync(command.Id)
            ?? throw new ArgumentException("User not found");

        return new UpdateAccountResult(
            UserId: user.CustomerId,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email,
            SocialSecurityNumber: user.SocialSecurityNumber
        );
    }
}
