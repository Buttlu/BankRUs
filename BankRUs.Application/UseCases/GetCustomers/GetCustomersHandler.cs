using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.GetCustomers;

public class GetCustomersHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<GetCustomersResult> HandleAsync(GetCustomersQuery query)
    {
        var pagedCustomers = await _customerService.GetAllAsync(query);

        return new GetCustomersResult(
            Customers: pagedCustomers.Data,
            PageMetaData: pagedCustomers.MetaData
        );
    }
}
