namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler
{
    public async Task<GetTransactionsResult> HandleAsync(GetTransactionsQuery query)
    {
        if (query.PageSize > 100) query.PageSize = 100;
    }
}
