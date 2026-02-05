namespace BankRUs.Application.Authentication.AuthenticateUser;

public record AuthenticatedUser(Guid UserId, string Username, string Email, IEnumerable<string> Roles = null!);