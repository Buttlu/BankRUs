namespace BankRUs.Application.Dtos.Customer;

public sealed record GetCustomersFiltersDto(
    // PAgination
    int Page,
    int PageSize,

    // Searching
    string? Ssn,
    string? Email
);
