using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenAccount;
// POST /api/accounts
public class OpenAccountHandler(
    IIdentityService identityService, 
    IBankAccountRepository bankAccountRepository,
    IEmailSender  emailSender,
    IAccountNumberGenerator accountNumberGenerator,
    IUnitOfWork unitOfWork
    )
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
            accountNumber: _accountNumberGenerator.Generate(),
            name: "standardkonto",
            userId: result.UserId.ToString());
        _bankAccountRepository.Add(bankAccount);
        await _unitOfWork.SaveAsync();

        // TODO: skick välkomstmail
        //      Delegera till infrastructure
        await _emailSender.SendEmailAsync(
            from: "no-rply@bankrus.com",
            to: command.Email,
            subject: "Account created",
            body: "Your account has been created"
            );

        return new OpenAccountResult(UserId: result.UserId);
    }
}
