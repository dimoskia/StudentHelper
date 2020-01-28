using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using StudentHelper.Data;
using StudentHelper.Models;
using System.Net.Http;

namespace JwtExample.Auth
{
    public class JwtAuthManager
    {
        public const string SecretKey = "JIOBLi6eVjBpvGtWBgJzjWd2QH0sOn5tI8rIFXSHKijXWEt/3J2jFYL79DQ1vKu+EtTYgYkwTluFRDdtF41yAQ==";
        
        public static string GenerateJWTToken(string email, DateTime expiration, StudentHelperContext db)
        {
            User user = db.Users.First(x => x.Email == email);
            var symmetric_Key = Convert.FromBase64String(SecretKey);
            var token_Handler = new JwtSecurityTokenHandler();

            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim("UserId", user.Id.ToString())
                        }),

                Expires = expiration,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetric_Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(SecretKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int GetUserIdFromRequest(HttpRequestMessage req)
        {
            ClaimsPrincipal principal = req.GetRequestContext().Principal as ClaimsPrincipal;
            var idString = principal.Claims.Where(c => c.Type == "UserId").Single().Value;
            return Convert.ToInt32(idString);
        }

        public static string GetEmailFromRequest(HttpRequestMessage req)
        {
            ClaimsPrincipal principal = req.GetRequestContext().Principal as ClaimsPrincipal;
            var email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
            return email;
        }
    }
}