namespace BankRUs.Application.Services;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken);
}
