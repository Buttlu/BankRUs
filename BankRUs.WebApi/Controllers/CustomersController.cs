using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = Roles.CustomerService)]
[ApiController]
public class CustomersController(GetCustomerHandler handler) : ControllerBase
{
    private readonly GetCustomerHandler _handler = handler;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        else if (pageSize > 100) pageSize = 100;

        // Result = Handler.Handle(Command)
        var result = await _handler.HandleAsync(new GetCustomerQuery(
            Page: page,
            PageSize: pageSize
        ));

        return Ok();
    }
}
