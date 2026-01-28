using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.WebApi.Dtos.BankAccounts;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankAccountsController(OpenBankAccountHandler handler) : ControllerBase
{
    private readonly OpenBankAccountHandler _handler = handler;

    [HttpPost]
    public async Task<IActionResult> AddAccount(CreateBankAccountRequestDto requestDto)
    {
        OpenBankAccountCommand command = new() {
            UserId = Guid.Parse(requestDto.UserId),
            AccountNumber = CreateAccountNumber(),
            AccountName = requestDto.AccountName ?? "Bank Account"
        };

        if (!ModelState.IsValid) {
            return ValidationProblem(ModelState);
        }

        OpenBankAccountResult result;
        try {
            result = await _handler.HandleAsync(command);
        } catch (Exception e) {
            ModelState.AddModelError("User Error", e.Message);
            return ValidationProblem(ModelState);
        }

        CreateBankAccountResponseDto response = new(result.AccountNumber);

        return Created("", new { response.AccountNumber });
    }

    private static string CreateAccountNumber()
    {
        Random rnd = new();
        return $"{rnd.Next(100, 1000)}.{rnd.Next(100, 1000)}.{rnd.Next(100, 1000)}";
    }
}