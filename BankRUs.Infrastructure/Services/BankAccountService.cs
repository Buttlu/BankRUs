using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Infrastructure.Services;

public class BankAccountService(
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork
) : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Add(BankAccount bankAccount)
    {
        _bankAccountRepository.Add(bankAccount);
        await _unitOfWork.SaveAsync();
    }

    public Task<BankAccount?> GetById(Guid bankAccountId)
        => _bankAccountRepository.GetById(bankAccountId);

    public Task<IReadOnlyList<BankAccount>> GetByUserId(Guid userId)
        => _bankAccountRepository.GetByUserId(userId);

    public async Task UpdateBalance(BankAccount bankAccount)
    {
        _bankAccountRepository.UpdateBalance(bankAccount);
        await _unitOfWork.SaveAsync();
    }
}
