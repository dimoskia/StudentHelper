using StudentHelper.Models;
using System.Net.Http;
using System.Net.Mail;
using System.Web;

namespace StudentHelper.Auth
{
    public class ConfirmationMail
    {
        public static void SendConfirmationEmail(User user, HttpRequestMessage req)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("internettehnologiiproekt@gmail.com");
            mailMessage.To.Add(user.Email);
            mailMessage.Subject = "StudentHelper Account Confirmation";
            mailMessage.IsBodyHtml = true;
            string name = user.UserDetails.FirstName + " " + user.UserDetails.LastName;
            string confirmationLink = string.Format("http://{0}:{1}/api/users/confirm?email={2}&code={3}",
                req.RequestUri.Host, req.RequestUri.Port, user.Email, HttpUtility.UrlEncode(user.ConfirmationCode));
            mailMessage.Body =
                "<hr>" +
                "<p></p>" +
                "<p></p>" +
                "<p>Hi " + name + ",</p>" +
                "<p></p>" +
                "<p>Thanks for creating a StudentHelper account. To continue, please confirm your email" +
                " address   by clicking the button below</p>" +
                "<p></p>" +
                "<p></p>" +
                "<a style='text-decoration: none; color: white;' href='" + confirmationLink + "'>" +
                "<div style='width: 200px; text-align: center; padding: 5px 2px 5px 2px; type:button; background: rgb(0, 204, 153); border-radius: 4px;'>Confirm email address</div>" +
                "</a>" +
                "<p></p>" +
                "<p></p>" +
                "<hr>";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "internettehnologiiproekt@gmail.com",
                Password = "ITPassword!"
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}