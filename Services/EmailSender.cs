using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Services
{
	public class EmailSender : IEmailSender
	{
        private readonly EmailSetting _settings;
        public EmailSender(IOptions<EmailSetting> options)
            => _settings = options.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
            var message = new MimeMessage();
            message.From.Add(
                new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            // Use HTML body
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                _settings.UseSSL
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto);
            await client.AuthenticateAsync(
                _settings.UserName,
                _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


	}
}






/*
 * using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace RatingManagementSystem.Services
{
	public class EmailSender : IEmailSender
	{
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
            var smtpClient = new SmtpClient("live.smtp.mailtrap.io")
            {
                Port = 587,
                Credentials = new NetworkCredential("smtp@mailtrap.io", "5387235b1e4bb92939181a18cb825198"),
                EnableSsl = true,
            };



            var message = new MailMessage();
			message.From = new MailAddress("testing@mailtrap.email");
            message.Subject = subject;
			message.Body = $"<html><body>{htmlMessage}</body></html>";
			message.IsBodyHtml = true;
            message.To.Add(email);


            smtpClient.Send(message);
			
		}
	}
}

*/
