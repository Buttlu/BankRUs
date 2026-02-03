namespace BankRUs.Infrastructure.Authentication;

public sealed record PaginationOptions
{
    public const string SectionName = "Paging";

    public int MaxPageSize { get; set; }
}
