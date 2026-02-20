using BankRUs.Application.Identity;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenAccount;
// POST /api/accounts
public class OpenAccountHandler(
    IIdentityService identityService, 
    IBankAccountService bankAccountService,
    IEmailSender  emailSender,
    IAccountNumberGenerator accountNumberGenerator
)
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;

    public async Task<OpenAccountResult> HandleAsync(OpenAccountCommand command, CancellationToken cancellationToken)
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
            accountNumber: _accountNumberGenerator.Generate(),
            name: "standardkonto",
            userId: result.UserId.ToString());
        await _bankAccountService.Add(bankAccount, cancellationToken);

        // TODO: skick välkomstmail
        //      Delegera till infrastructure
        await _emailSender.SendEmailAsync(
            from: "no-reply@bankrus.com",
            to: command.Email,
            subject: "Account created",
            body: "Your account has been created",
            cancellationToken: cancellationToken);

        return new OpenAccountResult(UserId: result.UserId);
    }
}
