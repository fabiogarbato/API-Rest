using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLogin login)
        {
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var token = GenerateJWTToken(user);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJWTToken(UserInfo userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserInfo? AuthenticateUser(UserLogin login) 
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                return new UserInfo { Username = login.Username, Email = "testuser@example.com" };
            }

            return null; 
        }
    }

    public class UserLogin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class UserInfo
    {
        public string? Username { get; set; } 
        public string? Email { get; set; } 
    }


}
