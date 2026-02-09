using BankRUs.Application.Services;
using BankRUs.Application.UseCases.DeleteCustomer;
using BankRUs.Application.UseCases.GetCustomers;
using BankRUs.Application.UseCases.UpdateAccount;
using BankRUs.Infrastructure.Authentication;
using BankRUs.Infrastructure.Identity;
using BankRUs.WebApi.Dtos.BankAccounts;
using BankRUs.WebApi.Dtos.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankRUs.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CustomersController(
    GetCustomersHandler getCustomersHandler,
    UpdateAccountHandler updateAccountHandler,
    DeleteCustomerHandler deleteCustomerHandler,
    IOptions<PaginationOptions> pageOptions,
    ICustomerService customerService

) : ControllerBase
{
    private readonly PaginationOptions _pageOptions = pageOptions.Value;
    private readonly GetCustomersHandler _getCustomersHandler = getCustomersHandler;
    private readonly UpdateAccountHandler _updateAccountHandler = updateAccountHandler;
    private readonly DeleteCustomerHandler _deleteCustomerHandler = deleteCustomerHandler;
    private readonly ICustomerService _customerService = customerService;

    [HttpGet]
    [Authorize(Roles = Roles.CustomerService)]
    public async Task<IActionResult> GetAll([FromQuery] CustomerRequestDto requestDto)
    {
        int page = requestDto.Page ?? 1;
        int pageSize = requestDto.PageSize ?? _pageOptions.DefaultPageSize;
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        else if (pageSize > _pageOptions.MaxPageSize) pageSize = _pageOptions.MaxPageSize;

        var result = await _getCustomersHandler.GetAllAsync(new GetCustomersQuery(
            Page: page,
            PageSize: pageSize,
            Ssn: requestDto.Ssn,
            Email: requestDto.Email
        ));

        var response = new GetAllCustomersResponseDto(
            Data: result.Customers,
            Page: result.PageMetaData.Page,
            PageSize: result.PageMetaData.PageSize,
            TotalItems: result.PageMetaData.TotalCount,
            TotalPages: result.PageMetaData.TotalPages
        );

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Roles.CustomerService)]
    public async Task<IActionResult> GetById(Guid id)
    {
        GetCustomerByIdResult result;
        try {
            result = await _getCustomersHandler.GetByIdAsync(new GetCustomerByIdQuery(
                UserId: id
            ));
        } catch (ArgumentException) {
            return NotFound();
        }

        var response = new GetCustomerByIdResponseDto(
            Id: result.UserId,
            FirstName: result.FirstName,
            LastName: result.LastName,
            Email: result.Email,
            SocialSecurityNumber: result.SocialSecurityNumber,
            BankAccounts: result.BankAccounts.Select(b => new BankAccountDto(
                Id: b.Id,
                Name: b.Name,
                AccountNumber: b.AccountNumber,
                Balance: b.Balance,
                UserId: null
            )).ToList()
        );

        return Ok(response);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = $"{Roles.Customer},{Roles.CustomerService}")]
    public async Task<IActionResult> Patch(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc
    )
    {
        if (patchDoc is null) return BadRequest();

        UpdateAccountResult result;
        try {
            result = await _updateAccountHandler.HandleAsync(new UpdateAccountCommand(
                Id: id,
                PatchDocument: patchDoc,
                ModelState: ModelState
            ));
        } catch (ArgumentException) {
            return NotFound();
        }

        var updateDto = new UpdateUserDto(
            FirstName: result.FirstName,
            LastName: result.LastName,
            Email: result.Email,
            SocialSecuritNumber: result.SocialSecurityNumber
        );

        patchDoc.ApplyTo(updateDto, ModelState);

        TryValidateModel(ModelState);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _customerService.UpdateCustomerInfo(result.UserId, updateDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerById(Guid id)
    {
        var result = await _deleteCustomerHandler.HandleAsync(new DeleteCustomerCommand(id));

        if (!result.Succeeded)
            return NotFound();

        return NoContent();
    }
}
