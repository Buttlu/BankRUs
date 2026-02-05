namespace BankRUs.WebApi.Dtos.Customer;

public sealed record CustomerRequestDto(
    // Pagination
    int? Page,
    int? PageSize,

    // search
    string? Ssn,
    string? Email
);
