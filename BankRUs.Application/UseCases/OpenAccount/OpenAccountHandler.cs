using BankRUs.Application.Identity;
using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.OpenAccount;
// POST /api/accounts
public class OpenAccountHandler(IIdentityService identityService, IBankAccountService bankAccountService)
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<OpenAccountResult> HandleAsync(OpenAccountCommand command)
    {
        // TODO: skapa konto (identity)
        //      Delegera till infrastructure
        CreateUserResult result = await _identityService.CreateUserAsync(new CreateUserRequest(
            FirstName: command.FirstName,
            LastName: command.LastName,
            Email: command.Email,
            SocialSecurityNumber: command.SocialSecurityNumber
        ));

        // TODO: skapa bankkonto
        //      Delegera till infrastructure
        Guid accountId = await _bankAccountService.CreateBankAccount(result.UserId);

        // TODO: skick välkomstmail
        //      Delegera till infrastructure

        return new OpenAccountResult(UserId: result.UserId);
    }
}
