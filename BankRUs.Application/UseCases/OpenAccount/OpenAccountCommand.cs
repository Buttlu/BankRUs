namespace BankRUs.Application.UseCases.OpenAccount;

public record OpenAccountCommand(
    string FirstName, 
    string LastName, 
    string SocialSecurityNumber, 
    string Email
);
