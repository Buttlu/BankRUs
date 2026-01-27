using BankRUs.Application.Identity;

namespace BankRUs.Application.UseCases.OpenAccount;
// POST /api/accounts
public class OpenAccountHandler(IIdentityService identityService)
{
    private readonly IIdentityService _identityService = identityService;

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

        // TODO: skick välkomstmail
        //      Delegera till infrastructure

        return new OpenAccountResult(UserId: result.UserId);
    }
}
