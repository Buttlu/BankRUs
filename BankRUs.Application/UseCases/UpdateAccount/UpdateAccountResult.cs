namespace BankRUs.Application.UseCases.UpdateAccount;

public sealed record UpdateAccountResult(
    Guid UserId,
    string FirstName,
    string LastName,
    string SocialSecurityNumber,
    string Email
);