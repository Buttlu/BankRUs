using BankRUs.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BankRUs.WebApi.Dtos.BankAccounts;

public record WithdrawBalanceDto(
    [MaxDecimals(2)]
    [Required]
    decimal Amount,
    [MaxLength(140)]
    [MinLength(1)]
    string Reference
);
