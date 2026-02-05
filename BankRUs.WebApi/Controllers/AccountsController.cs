using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.UpdateAccount;
using BankRUs.WebApi.Dtos.Accounts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(
    OpenAccountHandler openAccountHandler
) : ControllerBase
{
    private readonly OpenAccountHandler _openAccountHandler = openAccountHandler;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequestDto requestDto)
    {
        OpenAccountCommand command = new(
            FirstName: requestDto.FirstName,
            LastName: requestDto.LastName,
            SocialSecurityNumber: requestDto.SocialSecurityNumber,
            Email: requestDto.Email
        );
        
        if (!ModelState.IsValid) {
            return ValidationProblem(ModelState);
        }

        OpenAccountResult result;
        try {
            result = await _openAccountHandler.HandleAsync(command);
        } catch (Exception e) {
            ModelState.AddModelError("Duplicate Entry", e.Message);
            return ValidationProblem(ModelState);
        }

        CreateAccountResponseDto response = new(result.UserId);

        return Created("", new { response.UserId });
    }    
}
