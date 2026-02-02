using BankRUs.Infrastructure.Identity;
using BankRUs.WebApi.Dtos.Me;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = Roles.Customer)]
[ApiController]
public class MeController : ControllerBase
{
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
}
