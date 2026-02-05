namespace BankRUs.Application.UseCases.GetCustomers;

public sealed record GetCustomersQuery(
    // Pagination    
    int Page,
    int PageSize,

    // Search
    string? Ssn,
    string? Email
);
