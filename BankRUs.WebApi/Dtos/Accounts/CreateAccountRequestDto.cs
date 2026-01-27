using BankRUs.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BankRUs.WebApi.Dtos.Accounts;

public record CreateAccountRequestDto(
    [Required]
    [MaxLength(25, ErrorMessage = "First Name must be shorter than 25 characters")]
    string FirstName,
    
    [Required]
    [MaxLength(25, ErrorMessage = "Last Name must be shorter than 25 characters")]
    string LastName,
    
    [Required]
    [Personnummer]
    string SocialSecurityNumber,
    
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email
)
{
    public string FirstName { get; init; } = FirstName.Trim();
    public string LastName { get; init; } = LastName.Trim();
    public string SocialSecurityNumber { get; init; } = SocialSecurityNumber.Trim();
    public string Email { get; init; } = Email.Trim();
}