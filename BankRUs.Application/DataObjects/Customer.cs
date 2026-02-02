using BankRUs.Domain.Entities;

namespace BankRUs.Application.DataObjects;

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string SocialSecurityNumber { get; set; }
    public int NumberOfBankAccounts { get; set; }
    public IReadOnlyList<BankAccount> BankAccounts { get; set; }
}
