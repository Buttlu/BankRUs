namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler
{
    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        return new OpenBankAccountResult(Guid.NewGuid());
    }
}
