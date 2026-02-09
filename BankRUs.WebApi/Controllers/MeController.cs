using BankRUs.Application.UseCases.DeleteCustomer;
using BankRUs.Infrastructure.Identity;
using BankRUs.WebApi.Dtos.Me;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = Roles.Customer)]
[ApiController]
public class MeController(
    DeleteCustomerHandler deleteCustomerHandler
) : ControllerBase
{
    private readonly DeleteCustomerHandler _deleteCustomerHandler = deleteCustomerHandler;

    [Produces("application/json")]
    [ProducesResponseType(typeof(MeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // TODO GetAccountDetailsCommand(userId);

        var email = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.Identity?.Name ?? User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var response = new MeResponseDto(
            UserId: userId,
            Email: email ?? "",
            UserName: userName ?? ""
        );

        return Ok(response);
    }

    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete]
    public async Task<IActionResult> DeleteById()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var result = await _deleteCustomerHandler.HandleAsync(new DeleteCustomerCommand(Guid.Parse(userId)));
        
        if (!result.Succeeded) return NotFound();

        return NoContent();
    }
}
