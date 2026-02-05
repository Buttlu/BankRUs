namespace BankRUs.WebApi.Dtos.Customer;

public record UpdateUserDto(
    string? FirstName,
    string? LastName,
    string? SocialSecuritNumber,
    string? Email
);
