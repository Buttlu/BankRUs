namespace BankRUs.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
}
