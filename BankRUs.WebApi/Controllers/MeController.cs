using BankRUs.Application.UseCases.DeleteCustomer;
using BankRUs.Application.UseCases.GetMyDetails;
using BankRUs.Infrastructure.Identity;
using BankRUs.WebApi.Dtos.BankAccounts;
using BankRUs.WebApi.Dtos.Me;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = Roles.Customer)]
[ApiController]
public class MeController(
    DeleteCustomerHandler deleteCustomerHandler,
    GetMyDetailsHandler getMyDetailsHandler
) : ControllerBase
{
    private readonly DeleteCustomerHandler _deleteCustomerHandler = deleteCustomerHandler;
    private readonly GetMyDetailsHandler _getMyDetailsHandler = getMyDetailsHandler;

    [Produces("application/json")]
    [ProducesResponseType(typeof(MeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        GetMyDetailsResult result;
        try {
            result = await _getMyDetailsHandler.HandleAsync(new GetMyDetailsQuery(Guid.Parse(userId)), cancellationToken);
        } catch (ArgumentException ex) {
            ModelState.AddModelError("Account", ex.Message);
            return ValidationProblem(ModelState);
        }

        var response = new MeResponseDto(
            UserId: result.Id,
            Email: result.Email,
            UserName: result.FullName,
            SocialSecurityNumber: result.SocialSecurityNumber,
            BankAccounts: result.BankAccounts.Select(b => new CustomerBankAccountDto(
                Id: b.Id,
                Name: b.Name,
                AccountNumber: b.AccountNumber,
                Balance: b.Balance
            ))
            .ToList()
        );

        return Ok(response);
    }

    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var result = await _deleteCustomerHandler.HandleAsync(new DeleteCustomerCommand(Guid.Parse(userId)), cancellationToken);
        
        if (!result.Succeeded) return NotFound();

        return NoContent();
    }
}
