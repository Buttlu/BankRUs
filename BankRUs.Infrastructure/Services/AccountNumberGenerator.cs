using BankRUs.Application.Services;

namespace BankRUs.Infrastructure.Services;

public class AccountNumberGenerator : IAccountNumberGenerator
{
    Random rnd = new();
    public string Generate() => $"{rnd.Next(100, 1000)}.{rnd.Next(100, 1000)}.{rnd.Next(100, 1000)}";
}
