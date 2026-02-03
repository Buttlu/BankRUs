using BankRUs.Application.UseCases.AddBalance;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Application.UseCases.WithdrawBalance;
using BankRUs.WebApi.Dtos.BankAccounts;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.WebApi.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController(
    OpenBankAccountHandler bankAccountHandler, 
    AddBalanceHandler addBalanceHandler,
    WithdrawBalanceHandler withdrawBalanceHandler) 
    : ControllerBase
{
    private readonly OpenBankAccountHandler _bankAccountHandler = bankAccountHandler;
    private readonly AddBalanceHandler _addBalanceHandler = addBalanceHandler;
    private readonly WithdrawBalanceHandler _withdrawBalanceHandler = withdrawBalanceHandler;

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
        } catch (ArgumentException ae) { // Account not found
            ModelState.AddModelError("Account", ae.Message);
            return ValidationProblem(ModelState);
        } catch (ArithmeticException ae) { // Balance cannot be negative
            ModelState.AddModelError("Insuffient funds", ae.Message);
            return Conflict(ModelState);
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

    [HttpPost("{accoundId}/withdrawals")]
    public async Task<IActionResult> WithdrawBalance(Guid accoundId, [FromBody] WithdrawBalanceDto balanceDto)
    {
        if (balanceDto.Amount <= 0) {
            ModelState.AddModelError("Amount", "Amount must be above 0");
            return ValidationProblem(ModelState);
        }
        
        if (!ModelState.IsValid) {
            return ValidationProblem(ModelState);
        }

        WithdrawBalanceResult result = default!;
        try {
            result = await _withdrawBalanceHandler.HandleAsync(new WithdrawBalanceCommand(
                BankAccountId: accoundId,
                Amount: balanceDto.Amount,
                Reference: balanceDto.Reference
            ));
        } catch (ArgumentException ae) {
            ModelState.AddModelError("Input Data Error", ae.Message);
        }

        var resultDto = new WithdrawBalanceResultDto(
            TransactionId: result.TransactionId,
            UserId: result.UserId,
            Type: result.Type,
            Amount: result.Amount,
            Currency: result.Currency,
            Reference: result.Reference,
            CreatedAt: result.CreatedAt,
            BalanceAfter: result.BalanceAfter
        );

        return Created("", resultDto);
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetTransactionsFromAccount(Guid accountId, [FromQuery] TransactionQuery query)
    {

    }
}
