namespace BankRUs.Application.UseCases.DeleteCustomer;

public sealed record DeleteCustomerCommand(
    Guid CustomerId
);