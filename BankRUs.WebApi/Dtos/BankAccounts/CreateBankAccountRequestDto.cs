namespace BankRUs.WebApi.Dtos.BankAccounts;

public record CreateBankAccountRequestDto(
    Guid UserId,
    string? AccountName
);
