using BankRUs.Application.Identity;
using BankRUs.Application.Pagination;

namespace BankRUs.Application.UseCases.GetCustomers;

public sealed record GetCustomersResult(
    IReadOnlyList<CustomerDto> Customers,
    PageMetaData PageMetaData
);