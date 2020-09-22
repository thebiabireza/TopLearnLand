using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Context;

namespace TopLearnLand_Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly TopLearnLandContext _context;

        public EmailSenderService(TopLearnLandContext context)
        {
            _context = context;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string smtp = "";
            string userName = "";
            string password = "";
            string displayName = "";

            /*var c = _context.Config.Select(a => new { a.MailDisplayName, a.MailPassword, a.MailSmtpDomain, a.MailUserName }).AsNoTracking().FirstOrDefault();
            smtp = c.MailSmtpDomain;
            userName = c.MailUserName;
            password = c.MailPassword;
            displayName = c.MailDisplayName;*/

            MailMessage mail = new MailMessage();
            using (SmtpClient SmtpServer = new SmtpClient(smtp))
            {
                mail.From = new MailAddress(userName, displayName);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential(userName, password);
                SmtpServer.EnableSsl = true;
                await SmtpServer.SendMailAsync(mail);
            }
        }
    }
}
