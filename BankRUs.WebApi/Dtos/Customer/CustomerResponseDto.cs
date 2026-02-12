using BankRUs.Application.Identity;

namespace BankRUs.WebApi.Dtos.Customer;

public sealed record GetAllCustomersResponseDto(
    IReadOnlyList<CustomerDto> Data,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);
