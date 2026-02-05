using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler(
    IBankAccountRepository repository,
    IAccountNumberGenerator accountNumberGenerator,
    IUnitOfWork unitOfWork
)    
{
    private readonly IAccountNumberGenerator _accountNumberGenerator = accountNumberGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBankAccountRepository _bankAccountRepository = repository;

    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        Random rnd = new();
        
        var bankAccount = new BankAccount(
            accountNumber: _accountNumberGenerator.Generate(),
            name: command.AccountName ?? "standardkonto",
            userId: command.UserId.ToString());
        _bankAccountRepository.Add(bankAccount);
        await _unitOfWork.SaveAsync();

        return new OpenBankAccountResult(
            Id: bankAccount.Id,
            AccountNumber: bankAccount.AccountNumber,
            AccountName: bankAccount.Name,
            UserId: Guid.Parse(bankAccount.UserId)
        );
    }
}
