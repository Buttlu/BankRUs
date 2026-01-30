using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.WebApi.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(AuthenticateUserHandler authenticateUserHandler) : ControllerBase
{
    private readonly AuthenticateUserHandler _authenticateUserHandler = authenticateUserHandler;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
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
