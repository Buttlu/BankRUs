namespace BankRUs.Application.Pagination;

public sealed record PageMetaData(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);