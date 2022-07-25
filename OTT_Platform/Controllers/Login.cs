using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OTT_Platform.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using bcrypt = BCrypt.Net.BCrypt;

namespace OTT_Platform.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OTTPlatformDbContext _context;

        public LoginController(IConfiguration configuration, OTTPlatformDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<Profile>> Login([FromBody] Login login)
        {
            var checkEmail = await _context.Profiles.FirstOrDefaultAsync(x => x.Email == login.Email);
            if (checkEmail != null)
            {
                if (bcrypt.Verify(login.Password, checkEmail.Password))
                {
                    var token = CreateToken(checkEmail);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Wrong Password");
                }
            }
            else
            {
                return BadRequest("Invalid User");
            }
        }

        private string CreateToken(Profile profile)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("ID",profile.UserId.ToString()),
                new Claim(ClaimTypes.Email, profile.Email),
                new Claim(ClaimTypes.Role, profile.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("SecretKey:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
