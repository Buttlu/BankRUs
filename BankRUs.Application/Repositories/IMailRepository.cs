using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface IMailRepository
{
    Task SendAccountCreateEmail(string receiverMail, string fullCustomerName, BankAccount bankAccount);
}
