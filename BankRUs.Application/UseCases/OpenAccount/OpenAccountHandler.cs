using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenAccount;
// POST /api/accounts
public class OpenAccountHandler(
    IIdentityService identityService, 
    IBankAccountRepository bankAccountRepository,
    IMailRepository mailRepository
    )
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IMailRepository _mailRepository = mailRepository;

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
        var bankAccount = new BankAccount(
            accountNumber: "100.200.300",
            name: "standardkonto",
            userId: result.UserId.ToString());
        bankAccount = await _bankAccountRepository.CreateAsync(bankAccount);

        // TODO: skick välkomstmail
        //      Delegera till infrastructure
        await _mailRepository.SendAccountCreateEmail(command.Email, $"{command.FirstName} {command.LastName}", bankAccount);
        
        return new OpenAccountResult(UserId: result.UserId);
    }
}
