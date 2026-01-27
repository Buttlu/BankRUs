using BankRUs.Application.Services;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Infrastructure.Services;

public class BankAccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IBankAccountService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    public async Task<Guid> CreateBankAccount(Guid ownerId, decimal balance = 0.0m, string accountName = "Bank Account")
    {
        ApplicationUser owner = await _userManager.FindByIdAsync(ownerId.ToString()) 
            ?? throw new Exception("User not found");

        BankAccount bankAccount = new() {
            AccountNumberId = Guid.NewGuid(),
            Balance = balance,
            Name = accountName,
            OwnerId = ownerId.ToString()
        };
        
        owner.BankAccounts.Add(bankAccount);
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
        return bankAccount.AccountNumberId;
    }
}
