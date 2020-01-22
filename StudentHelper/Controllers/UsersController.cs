using StudentHelper.Models.DTOs;
using System.Security.Cryptography;
using System.Web.Http;

namespace StudentHelper.Controllers
{
    public class UsersController : ApiController
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public IHttpActionResult PostNewUser(NewUserRequest userRequest)
        {
            
            return Ok();
        }
    }
}