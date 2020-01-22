using JwtExample.Auth;
using StudentHelper.Data;
using StudentHelper.Models;
using StudentHelper.Models.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.Http;

namespace StudentHelper.Controllers
{
    public class UsersController : ApiController
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private StudentHelperContext db = new StudentHelperContext();

        [Route("api/users/signup")]
        public IHttpActionResult PostNewUser(UserDTO userRequest)
        {
            if (!IsEmailAvailable(userRequest.Email))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
                {
                    Content = new StringContent("Account with the given email already exists")
                };
                throw new HttpResponseException(resp);
            }

            byte[] salt;
            rngCsp.GetBytes(salt = new byte[16]);   // generate salt

            var pdkdf2 = new Rfc2898DeriveBytes(userRequest.Password, salt, 1000);
            byte[] hash = pdkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            byte[] confirmationCode;
            rngCsp.GetBytes(confirmationCode = new byte[10]);

            User user = new User {
                Email = userRequest.Email,
                Password = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(salt),
                Role = "unconfirmed",
                ConfirmationCode = Convert.ToBase64String(confirmationCode),
                UserDetails = new UserDetails { FirstName = userRequest.FirstName, LastName = userRequest.LastName }
            };

            db.Users.Add(user);
            db.SaveChanges();

            SendConfirmationEmail(user, Request);

            return Ok("Account successfully created");
        }

        private void SendConfirmationEmail(User user, HttpRequestMessage req)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("internettehnologiiproekt@gmail.com");
            mailMessage.To.Add(user.Email);
            mailMessage.Subject = "StudentHelper Account Confirmation";
            mailMessage.IsBodyHtml = true;
            string name = user.UserDetails.FirstName + " " + user.UserDetails.LastName;
            string confirmationLink = string.Format("http://{0}:{1}/api/users/confirm?email={2}&code={3}",
                req.RequestUri.Host, req.RequestUri.Port, user.Email, user.ConfirmationCode);
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

        [Route("api/users/confirm")]
        public IHttpActionResult GetConfirmation(string email, string code)
        {
            User user = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            if(user == null || !user.ConfirmationCode.Equals(code))
            {
                return BadRequest();
            }
            user.Role = "user";
            db.SaveChanges();
            return Redirect("http://localhost:3000/login");
        }

        [Route("api/users/signin")]
        public HttpResponseMessage PostRequestToken(UserDTO userDto)
        {

            if(CheckCredentials(userDto.Email, userDto.Password))
            {
                return Request.CreateResponse(HttpStatusCode.OK, JwtAuthManager.GenerateJWTToken(userDto.Email));
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Email or password is incorrect");
        }

        private bool CheckCredentials(string email, string password)
        {
            User user = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if(user == null)
            {
                return false;
            }
            
            byte[] salt = Convert.FromBase64String(user.Salt);

            var pdkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);

            byte[] hash = pdkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string passwordHashed = Convert.ToBase64String(hashBytes);

            return user.Password.Equals(passwordHashed);
        }

        private bool IsEmailAvailable(string email)
        {
            return db.Users.Count(user => user.Email.Equals(email)) == 0;
        }
    }
}
