using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.GetCustomers;

public sealed record GetCustomerByIdResult(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string SocialSecurityNumber,
    IReadOnlyList<BankAccount> BankAccounts
);