using JwtExample.Auth;
using StudentHelper.Data;
using StudentHelper.Models;
using StudentHelper.Models.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            User user = new User {
                Email = userRequest.Email,
                Password = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(salt),
                Role = "user",
                Confirmed = true,
                UserDetails = new UserDetails { FirstName = userRequest.FirstName, LastName = userRequest.LastName }
            };

            db.Users.Add(user);
            db.SaveChanges();

            return Ok("Account successfully created");
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
