using System.ComponentModel.DataAnnotations.Schema;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public Guid AccountNumberId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    [ForeignKey("ApplicationUserId")]
    public string OwnerId { get; set; } = string.Empty;
}
