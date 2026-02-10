using BankRUs.WebApi.Dtos.BankAccounts;

namespace BankRUs.WebApi.Dtos.Me;

public sealed record MeResponseDto(
    Guid UserId, 
    string Email, 
    string UserName,
    string SocialSecurityNumber,
    IReadOnlyList<CustomerBankAccountDto> BankAccounts
);