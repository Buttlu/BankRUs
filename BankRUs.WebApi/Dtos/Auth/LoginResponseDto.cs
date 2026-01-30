using System.ComponentModel.DataAnnotations;

namespace BankRUs.WebApi.Dtos.Auth;

public record LoginResponseDto(
    [Required]
    string? Token,
    [Required]
    DateTime? ExpiredAtUtc
);