using BankRUs.Application.Services;
using BankRUs.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler(
    IAccountNumberGenerator accountNumberGenerator,
    ICustomerService customerService,
    IBankAccountService bankAccountService,
    ILogger<OpenBankAccountHandler> logger
)    
{
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;
    private readonly ICustomerService _customerService = customerService;
    private readonly IBankAccountService _bankAccountService = bankAccountService;
    private readonly ILogger<OpenBankAccountHandler> _logger = logger;

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
        _logger.LogInformation("Created User {UserId} with bank account {BankAccountId}", customer.CustomerId, bankAccount.Id);

        return new OpenBankAccountResult(
            Id: bankAccount.Id,
            AccountNumber: bankAccount.AccountNumber,
            AccountName: bankAccount.Name,
            UserId: customer.CustomerId
        );
    }
}
