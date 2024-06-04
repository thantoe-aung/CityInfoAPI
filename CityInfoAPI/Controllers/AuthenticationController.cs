using CityInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CityInfoAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        public class AuthenticationRequestBody
        {
            public string?  UserName { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = ValidateCredentials(authenticationRequestBody.UserName,authenticationRequestBody.Password);
            if(user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenService.SecretForKey));
            var singingCredientials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.UserId.ToString()));
            claims.Add(new Claim("given_name", user.FirstName ?? ""));
            claims.Add(new Claim("city", user.City ?? ""));

            var token = new JwtSecurityToken(TokenService.Issuer,TokenService.Audience,claims,
                DateTime.UtcNow,DateTime.UtcNow.AddHours(1),singingCredientials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenToReturn);

        }

        private CityInfoUser ValidateCredentials(string? userName,string? password)
        {
            return new CityInfoUser(1, userName ?? "", "John", "Kavin", "Yangoon");
        }

        public class CityInfoUser
        {
            public int UserId { get; set; }

            public string UserName { get; set; }

            public string? FirstName { get; set; }

            public string? LastName { get; set;}

            public string? City { get; set; }

            public CityInfoUser(int userId,string userName,string firstName,string lastName,string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName  = lastName;
                City = city;
            }

        }
    }
}
