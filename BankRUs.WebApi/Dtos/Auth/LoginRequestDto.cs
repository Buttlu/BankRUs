using System.ComponentModel.DataAnnotations;

namespace BankRUs.WebApi.Dtos.Auth;

public record LoginRequestDto(
    [Required]
    string Username,
    [Required]
    string Password
);