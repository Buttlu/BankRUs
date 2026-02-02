using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.GetCustomers;

public class GetCustomerHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<GetCustomerResult> HandleAsync(GetCustomerQuery query)
    {
        // 1. get all from service
        var customers = await _customerService.GetAll(query.Page, query.PageSize);

        return new GetCustomerResult {

        };
    }
}
