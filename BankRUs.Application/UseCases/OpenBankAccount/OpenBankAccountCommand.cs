namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountCommand
{
    public required Guid UserId { get; set; }
    public required string AccountNumber { get; set; }
    public string AccountName { get; set; }
}
