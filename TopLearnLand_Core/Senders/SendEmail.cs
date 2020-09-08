using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TopLearnLand_Core.Senders
{
    public class SendEmail
    {
        public static void Send(string To, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("xrezab.a.b0016@gmail.com", "تاپ لرن لند");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("xrezab.a.b0016@gmail.com", "rezabiabi0016199877");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}