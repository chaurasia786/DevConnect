using Microsoft.Extensions.Options;
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


        


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var fromAddress = new MailAddress("shankarlal.chaurasiya@neosoftmail.com", "Neo Mail");
                var toAddress = new MailAddress(email);
                const string smtpUser = "shankarlal.chaurasiya@neosoftmail.com";
                const string smtpPassword = "c!ljWpLh2sPc"; 

                var smtp = new SmtpClient
                {
                    Host = "mail.neosoftmail.com", 
                    Port = 25, 
                    EnableSsl = false,
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
