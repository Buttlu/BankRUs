using System.ComponentModel.DataAnnotations;

namespace BankRUs.WebApi.Dtos.BankAccounts;

public record BankAccountDto(
    Guid Id, 
    string Name, 
    string AccountNumber, 
    decimal Balance, 
    Guid UserId
);