using JwtExample.Auth;
using StudentHelper.Auth;
using StudentHelper.Data;
using StudentHelper.Models;
using StudentHelper.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
                    Content = new StringContent("Веќе постои корисник со внесената email адреса")
                };
                throw new HttpResponseException(resp);
            }

            byte[] salt;
            rngCsp.GetBytes(salt = new byte[16]);

            var pdkdf2 = new Rfc2898DeriveBytes(userRequest.Password, salt, 1000);
            byte[] hash = pdkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // byte[] confirmationCode;
            // rngCsp.GetBytes(confirmationCode = new byte[10]);

            User user = new User {
                Email = userRequest.Email,
                Password = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(salt),
                Role = "user",
                // ConfirmationCode = Convert.ToBase64String(confirmationCode),
                UserDetails = new UserDetails { FirstName = userRequest.FirstName, LastName = userRequest.LastName }
            };

            db.Users.Add(user);
            db.SaveChanges();

            // ConfirmationMail.SendConfirmationEmail(user, Request);

            return Ok("Account successfully created");
        }

        //[Route("api/users/confirm")]
        //public IHttpActionResult GetConfirmation(string email, string code)
        //{
        //    User user = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
        //    if(user == null || !user.ConfirmationCode.Equals(code))
        //    {
        //        return BadRequest();
        //    }
        //    user.Role = "user";
        //    db.SaveChanges();
        //    return Redirect("");
        //}

        [Route("api/users/signin")]
        public HttpResponseMessage PostRequestToken(UserDTO userDto)
        {

            if(CheckCredentials(userDto.Email, userDto.Password))
            {
                DateTime expiration = DateTime.UtcNow.AddHours(1);
                string token = JwtAuthManager.GenerateJWTToken(userDto.Email, expiration);
                User user = db.Users.Where(u => u.Email.Equals(userDto.Email)).FirstOrDefault();
                user.FavouritesIds = user.Favorites.Select(f => f.Id).ToList();

                long expirationMillis = Convert.ToInt64(
                    expiration
                        .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                        .TotalMilliseconds
                );

                var resp = new SuccessfulSignInDTO
                {
                    Token = token,
                    Expiration = expirationMillis,
                    User = user
                };

                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Внесената email адреса или лозинка е погрешна");
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

        [Route("api/users/changePassword")]
        [JwtAuthentication]
        public HttpResponseMessage PostChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            string email = JwtAuthManager.GetEmailFromRequest(Request);
            if (CheckCredentials(email, changePasswordDTO.Password))
            {
                int userId = JwtAuthManager.GetUserIdFromRequest(Request);
                User user = db.Users.Find(userId);

                byte[] salt;
                rngCsp.GetBytes(salt = new byte[16]);

                var pdkdf2 = new Rfc2898DeriveBytes(changePasswordDTO.NewPassword, salt, 1000);
                byte[] hash = pdkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                user.Password = Convert.ToBase64String(hashBytes);
                user.Salt = Convert.ToBase64String(salt);

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Промената на лозинка е неуспешна, бидејќи внесовте погрешна стара лозинка");
        }

        [Route("api/users/deactivateAccount")]
        [JwtAuthentication]
        public HttpResponseMessage PostDeactivateAccount(UserDTO userDTO)
        {
            string email = JwtAuthManager.GetEmailFromRequest(Request);
            if (CheckCredentials(email, userDTO.Password))
            {
                int userId = JwtAuthManager.GetUserIdFromRequest(Request);
                User user = db.Users.Find(userId);

                List<Comment> allComments = db.Comments.Where(c => c.UserDetails.UserDetailsId == userId).ToList();
                allComments.ForEach(c => db.Comments.Remove(c));

                int? imageId = Image.ExtractImageId(user.UserDetails.ImageUrl);

                if (imageId != null)
                {
                    ImageController.DeleteImage(imageId.Value, db);
                }

                db.UserDetails.Remove(user.UserDetails);

                db.Users.Remove(user);

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Профилот не е деактивиран бидејќи лозинката која ја внесовте е погрешна.");
        }

        // PATCH: api/Users/ChangeImage
        [Route("api/users/changeImage")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> PatchUser()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            int userId = JwtAuthManager.GetUserIdFromRequest(Request);
            UserDetails userDetails = db.UserDetails.Find(userId);
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            var imageBytes = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

            int? oldImageId = Image.ExtractImageId(userDetails.ImageUrl);

            if (oldImageId != null)
            {
                ImageController.DeleteImage(oldImageId.Value, db);
            }

            userDetails.ImageUrl = ImageController.SaveImage(imageBytes, Request, db);

            db.SaveChanges();

            return Ok(userDetails);
        }
    }
}
