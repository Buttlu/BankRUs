using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public BankAccount(string accountNumber, string name, string userId)
    {
        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        Name = name;
        UserId = userId;
    }

    [Key]
    public Guid Id { get; protected set; }
    
    [MaxLength(25)]
    public string Name { get; protected set; }
    
    [MaxLength(25)]
    public string AccountNumber { get; protected set; }
    
    public decimal Balance { get; protected set; }
    
    public string UserId { get; protected set; } 

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (Balance - Balance < 0) {
            throw new ArgumentException("Balance cannot be negative");
        }
        Balance -= amount;
    }
}
