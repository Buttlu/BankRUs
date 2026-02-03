namespace BankRUs.Application.Pagination;

public sealed record PagedResponse<T>(
    IReadOnlyList<T> Data,
    PageMetaData MetaData
);