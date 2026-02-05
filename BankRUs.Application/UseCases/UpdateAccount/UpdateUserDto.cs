namespace BankRUs.Application.UseCases.UpdateAccount;

public record UpdateUserDto(
    string? FirstName,
    string? LastName,
    string? SocialSecuritNumber,
    string? Email
);
