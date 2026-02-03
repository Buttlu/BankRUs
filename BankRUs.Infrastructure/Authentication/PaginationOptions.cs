namespace BankRUs.Infrastructure.Authentication;

public sealed record PaginationOptions
{
    public const string SectionName = "Paging";

    public int MaxPageSize { get; set; }
    public int DefaultPageSize { get; set; }
}
