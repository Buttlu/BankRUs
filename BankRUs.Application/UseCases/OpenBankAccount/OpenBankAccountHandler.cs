using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler(IBankAccountRepository bankAccountRepository)
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;

    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        BankAccount bankAccount = new(
            accountNumber: command.AccountNumber,
            name: command.AccountName,
            userId: command.UserId.ToString());
        bankAccount = await _bankAccountRepository.CreateAsync(bankAccount);

        return new() { AccountNumber = bankAccount.AccountNumber };
    }
}
