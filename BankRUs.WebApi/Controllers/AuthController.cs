using BankRUs.Application.Authentication;
using BankRUs.WebApi.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(AuthenticateUserHandler authenticateUserHandler) : ControllerBase
{
    private readonly AuthenticateUserHandler _authenticateUserHandler = authenticateUserHandler;

    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authenticateUserHandler.HandleAsync(new AuthenticateUserCommand(
            request.Username,
            request.Password));

        // 401 - unauthorized
        if (!result.Succeed) 
            return Unauthorized();

        var response = new LoginResponseDto(
            Token: result.AccessToken,
            ExpiredAtUtc: result.ExpiredAtUtc);

        return Ok(response);
    }
}
