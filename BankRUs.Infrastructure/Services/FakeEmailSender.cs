using BankRUs.Application.Services;
using System.Net.Mail;

namespace BankRUs.Infrastructure.Services;

public class FakeEmailSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        SmtpClient client = new("localhost", 25);
        await client.SendMailAsync(new MailMessage(from, to, subject, body));
    }    
}
