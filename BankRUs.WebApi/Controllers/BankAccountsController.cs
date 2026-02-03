using BankRUs.Application.UseCases.AddBalance;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.WebApi.Dtos.BankAccounts;
using Bogus.DataSets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.Xml;

namespace BankRUs.WebApi.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController(OpenBankAccountHandler bankAccountHandler, AddBalanceHandler addBalanceHandler) : ControllerBase
{
    private readonly OpenBankAccountHandler _bankAccountHandler = bankAccountHandler;
    private readonly AddBalanceHandler _addBalanceHandler = addBalanceHandler;

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestDto request)
    {
        OpenBankAccountResult openBankAccountResult = await _bankAccountHandler.HandleAsync(new OpenBankAccountCommand(
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

    [HttpPost("{accoundId}/deposits")]
    public async Task<IActionResult> AddBalance(Guid accoundId, [FromBody] AddBalanceDto addBalanceDto)
    {
        if (addBalanceDto.Amount <= 0) {
            ModelState.AddModelError("Amount", "Amount must be above 0");
            return ValidationProblem(ModelState);
        }
        if (!ModelState.IsValid) {
            return ValidationProblem(ModelState);
        }

        AddBalanceResult balanceResult = default!;
        try {
            balanceResult = await _addBalanceHandler.HandleAsync(new AddBalanceCommand(
                BankAccountId: accoundId,
                Amount: addBalanceDto.Amount,
                Reference: addBalanceDto.Reference
            ));
        } catch (ArgumentException ae) {
            ModelState.AddModelError("Account", ae.Message);
        }

        var resultDto = new AddBalanceResultDto(
            TransactionId: balanceResult.TransactionId,
            UserId: balanceResult.UserId,
            Type: balanceResult.Type,
            Amount: balanceResult.Amount,
            Currency: balanceResult.Currency,
            Reference: balanceResult.Reference,
            CreatedAt: balanceResult.CreatedAt,
            BalanceAfter: balanceResult.BalanceAfter
        );

        return Created("", resultDto);
    }
}
