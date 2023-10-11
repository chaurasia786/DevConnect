//using MailKit.Net.Smtp;
//using MailKit.Security;
using Microsoft.Extensions.Options;
//using MimeKit;

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NetNestConnect.Model
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    try
        //    {
        //        var mimeMessage = new MimeMessage();
        //        mimeMessage.From.Add(new MailboxAddress("Shankar Chaurasia", "chaurasia.shankar@gmail.com")); // Use a demo sender name and email
        //        mimeMessage.To.Add(new MailboxAddress(email, email));
        //        mimeMessage.Subject = subject;
        //        mimeMessage.Body = new TextPart("plain") { Text = message };

        //        using (var client = new SmtpClient())
        //        {
        //            // Mailtrap SMTP settings
        //            string mailtrapHost = "smtp.mailtrap.io";
        //            int mailtrapPort = 2525;
        //            string mailtrapUser = "0326afb4bbd6d7";
        //            string mailtrapPassword = "ecde0f9ff736d0";

        //            await client.ConnectAsync(mailtrapHost, mailtrapPort, SecureSocketOptions.Auto);
        //            await client.AuthenticateAsync(mailtrapUser, mailtrapPassword);
        //            await client.SendAsync(mimeMessage);
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var fromAddress = new MailAddress("shankarlal.chaurasiya@neosoftmail.com", "Neo Mail");
                var toAddress = new MailAddress(email);
                const string smtpUser = "shankarlal.chaurasiya@neosoftmail.com"; // Your SMTP username (usually the same as the 'from' address)
                const string smtpPassword = "c!ljWpLh2sPc"; // Your SMTP password

                var smtp = new SmtpClient
                {
                    Host = "mail.neosoftmail.com", // Your SMTP server address
                    Port = 25, // Standard SMTP port
                    EnableSsl = false, // Depending on your server's configuration, you may need to set this to 'true'
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword)
                };

                using (var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message
                })
                {
                    await smtp.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }

}
