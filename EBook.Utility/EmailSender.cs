using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EBook.Utility;
public class EmailSender : IEmailSender
{
    private readonly string SendGridSecret = string.Empty;
    public EmailSender(IConfiguration configuration)
    {
        SendGridSecret = configuration.GetValue<string>("SendGrid:SecretKey");
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(SendGridSecret);
        var from = new EmailAddress("mahmmoudkinawy@gmail.com", "EBook");
        var to = new EmailAddress(email);
        var message = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlMessage);

        return client.SendEmailAsync(message);
    }
}
