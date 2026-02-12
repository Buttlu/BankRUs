using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BankRUs.Application.UseCases.UpdateAccount;

public sealed record UpdateAccountCommand(
    Guid Id,
    JsonPatchDocument<UpdateUserDto> PatchDocument,
    ModelStateDictionary ModelState
);