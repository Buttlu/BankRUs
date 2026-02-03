namespace BankRUs.Application.UseCases.GetCustomers;

public sealed record GetCustomersQuery(
    int Page,
    int PageSize
);
