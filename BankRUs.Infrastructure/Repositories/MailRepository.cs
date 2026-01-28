using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace BankRUs.Infrastructure.Repositories;

public class MailRepository(
    SmtpClient smtpClient, 
    UserManager<ApplicationUser> userManager
    ) : IMailRepository
{
    private readonly SmtpClient _smtpClient = smtpClient;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task SendAccountCreateEmail(string receiverMail, string fullCustomerName, BankAccount bankAccount)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(receiverMail)
            ?? throw new ArgumentException($"User with mail {receiverMail} not found");        
        MailMessage message = new();
        message.To.Add(new MailAddress(receiverMail));
        message.From = new MailAddress("no-reply@bankrus.com");
        message.Subject = "Account Created";
        message.Body = $"""
            Hello {fullCustomerName}! Thank you for becoming a customer at Bank-R-Us!
            
            Your Id: {user.Id}
            Your Bank Account Number: {bankAccount.AccountNumber}
            
            Cheers,
            Bank-R-Us
            """;

        _smtpClient.Host = "localhost";
        _smtpClient.Port = 25;
        await _smtpClient.SendMailAsync(message);
    }
}
