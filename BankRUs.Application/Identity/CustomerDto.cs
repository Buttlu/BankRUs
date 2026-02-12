using BankRUs.Domain.Entities;

namespace BankRUs.Application.Identity;

public record CustomerDto(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string SocialSecurityNumber,
    IReadOnlyList<BankAccount>? BankAccounts
);