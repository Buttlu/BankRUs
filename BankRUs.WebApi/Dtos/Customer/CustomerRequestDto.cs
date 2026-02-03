namespace BankRUs.WebApi.Dtos.Customer;

public sealed record CustomerRequestDto(
    int? Page,
    int? PageSize
);
