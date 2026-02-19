using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BankRUs.Infrastructure.Services;

public class BankAccountService(
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork,
    ILogger<BankAccountService> logger
) : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<BankAccountService> _logger = logger;

    public async Task Add(BankAccount bankAccount, CancellationToken cancellationToken)
    {
        _bankAccountRepository.Add(bankAccount);
        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogInformation("Created Bank Account {BankAccountId}", bankAccount.Id);
    }

    public async Task<BankAccount?> GetById(Guid bankAccountId, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository.GetById(bankAccountId, cancellationToken);
        if (bankAccount is null) {
            _logger.LogWarning("Bank Account with Id: {BankAccountId} not found", bankAccountId);
            return null;
        }
        _logger.LogInformation("Bank Account with Id: {BankAccountId}", bankAccountId);
        return bankAccount;
    }

    public async Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var bankAccounts = await _bankAccountRepository.GetByUserId(userId, cancellationToken);
        _logger.LogInformation("Found {BankAccountCount} belonging to user: {UserId}", bankAccounts.Count, userId);
        return bankAccounts;
    }

    public async Task UpdateBalance(BankAccount bankAccount, CancellationToken cancellationToken)
    {
        _bankAccountRepository.UpdateBalance(bankAccount);
        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogInformation("Updated Bank Account: {BankAccountId}", bankAccount.Id);
    }
}
