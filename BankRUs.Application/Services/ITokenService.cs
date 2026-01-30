namespace BankRUs.Application.Services;

public interface ITokenService
{
    Token CreateToken(string UserId, string Email);
}
