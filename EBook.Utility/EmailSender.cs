using Microsoft.AspNetCore.Identity.UI.Services;

namespace EBook.Utility;
public class EmailSender : IEmailSender
{
    //Fake implemention just for stop the error
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
        => Task.CompletedTask;
}
