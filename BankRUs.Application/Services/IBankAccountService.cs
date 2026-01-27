namespace BankRUs.Application.Services;

public interface IBankAccountService
{
    Task<Guid> CreateBankAccount(Guid ownerId, decimal balance = 0.0m, string accountName = "Bank Account");
}
