using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler(
    IAccountNumberGenerator accountNumberGenerator,
    ICustomerService customerService,
    IBankAccountService bankAccountService
)    
{
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;
    private readonly ICustomerService _customerService = customerService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;

    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        var customer = await _customerService.GetByIdAsync(command.UserId)
            ?? throw new ArgumentException("Cannot find customer");

        var bankAccount = new BankAccount(
            accountNumber: _accountNumberGenerator.Generate(),
            name: command.AccountName ?? "standardkonto",
            userId: customer.CustomerId.ToString()
        );

        await _bankAccountService.Add(bankAccount);

        return new OpenBankAccountResult(
            Id: bankAccount.Id,
            AccountNumber: bankAccount.AccountNumber,
            AccountName: bankAccount.Name,
            UserId: customer.CustomerId
        );
    }
}
