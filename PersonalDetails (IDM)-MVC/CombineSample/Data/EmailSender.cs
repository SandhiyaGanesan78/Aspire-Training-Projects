using CombineSample.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
           var mimeMessage = new MimeMessage();
           mimeMessage.From.Add(new MailboxAddress("sandhiya", "sandhiyaganesan76@gmail.com"));
           mimeMessage.To.Add(new MailboxAddress("",email));
           mimeMessage.Subject = "Reset Password";
           mimeMessage.Body = new TextPart("plain")
           {
            Text = "Please reset your password by clicking the link below."
            };
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sandhiyaganesan76@gmail.com", "xqjefzgxjgqxkgqn");
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
            Console.WriteLine("sented");
        }
        }
        
        catch (Exception ex)
        {
            throw new ApplicationException($"Error sending email. Error: {ex.Message}");
        }
    }
}
