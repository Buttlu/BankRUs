using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.GetMyDetails;

public sealed record GetMyDetailsResult(
    Guid Id,
    string Email, 
    string FullName,
    string SocialSecurityNumber,
    IReadOnlyList<BankAccount> BankAccounts
);