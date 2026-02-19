using BankRUs.Application.Services;

namespace BankRUs.Application.UseCases.DeleteCustomer;

public class DeleteCustomerHandler(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<DeleteCustomerResult> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        bool succeeded = await _customerService.DeleteCustomer(command.CustomerId, cancellationToken);
        
        return new DeleteCustomerResult(
            succeeded
        );
    }
}
