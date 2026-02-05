using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.WebApi.Dtos.Accounts;
using BankRUs.WebApi.Dtos.Customer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(OpenAccountHandler handler) : ControllerBase
{
    private readonly OpenAccountHandler _openAccountHandler = handler;

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

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc
    )
    {
        if (patchDoc is null) return BadRequest();

        var customer = 
    }
}
