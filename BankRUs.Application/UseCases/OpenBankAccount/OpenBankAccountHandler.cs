using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler(
    IBankAccountRepository repository,
    IAccountNumberGenerator accountNumberGenerator
)    
{
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;
    private readonly IBankAccountRepository _bankAccountRepository = repository;

    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        Random rnd = new();
        
        var bankAccount = new BankAccount(
            accountNumber: _accountNumberGenerator.Generate(),
            name: command.AccountName ?? "standardkonto",
            userId: command.UserId.ToString());
        await _bankAccountRepository.AddAsync(bankAccount);
        
        return new OpenBankAccountResult(
            Id: bankAccount.Id,
            AccountNumber: bankAccount.AccountNumber,
            AccountName: bankAccount.Name,
            UserId: Guid.Parse(bankAccount.UserId)
        );
    }
}
