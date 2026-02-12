namespace BankRUs.Application.Authentication;

public sealed record AuthenticateUserResult
{
    public static AuthenticateUserResult Succeeded(string accessToken, DateTime expiredAtUtc)
        => new() {
            Succeed = true,
            AccessToken = accessToken,
            ExpiredAtUtc = expiredAtUtc
        };

    public static AuthenticateUserResult Failed() 
        => new() {
            Succeed = false
        };
    public required bool Succeed { get; set; }

    public string? AccessToken { get; set; }
    public DateTime? ExpiredAtUtc { get; set; }
}