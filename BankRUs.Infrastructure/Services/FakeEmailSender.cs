using BankRUs.Application.Services;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace BankRUs.Infrastructure.Services;

public class FakeEmailSender(ILogger<FakeEmailSender> logger) : IEmailSender
{
    private readonly ILogger<FakeEmailSender> _logger = logger;

    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        SmtpClient client = new("localhost", 25);
        await client.SendMailAsync(new MailMessage(from, to, subject, body));
        _logger.LogInformation("Sent email to {UserEmail}", to);
    }    
}
