using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.WebApi.Dtos.BankAccounts;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController(OpenBankAccountHandler handler) : ControllerBase
{
    private readonly OpenBankAccountHandler _handler = handler;

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestDto request)
    {
        OpenBankAccountResult openBankAccountResult = await _handler.HandleAsync(new OpenBankAccountCommand(
                UserId: request.UserId,
                AccountName: request.AccountName
            )
        );

        BankAccountDto response = new(
            Id: openBankAccountResult.Id,
            Name: openBankAccountResult.AccountName,
            AccountNumber: openBankAccountResult.AccountNumber,
            Balance: 0.0m,
            UserId: openBankAccountResult.UserId
            );

        return Created("", response);
    }    
}
