using BankRUs.WebApi.Dtos.BankAccounts;

namespace BankRUs.WebApi.Dtos.Customer;

public sealed record GetCustomerByIdResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string SocialSecurityNumber,
    IReadOnlyList<BankAccountDto> BankAccounts
);
