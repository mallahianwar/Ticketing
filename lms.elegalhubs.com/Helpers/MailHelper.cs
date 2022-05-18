using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Helpers
{
    public class MailHelper
    {
        public static bool SendEmail(string Subject, string To, string Message)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                var mail = new MailMessage();
                mail.From = new MailAddress("ellegalhub@gmail.com", "El-Legal Hub", Encoding.UTF8);
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = Message;
                mail.Body = htmlBody;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ellegalhub@gmail.com", "El@2021#HH");
                SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryFormat = SmtpDeliveryFormat.International;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
