using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTT_Platform.Models;
using bcrypt = BCrypt.Net.BCrypt;

namespace Grievence_Management.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly OTTPlatformDbContext _context;

        public RegisterController(IConfiguration configuration, OTTPlatformDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Profile>> Register([FromBody] Profile profile)
        {
            var checkEmail = await _context.Profiles.FirstOrDefaultAsync(x => x.Email == profile.Email);
            if (checkEmail == null)
            {
                if (profile.Email.Contains("@gmail.com") && profile.Password.Length > 8)
                {
                    profile.Password = bcrypt.HashPassword(profile.Password, 12);
                    _context.Profiles.Add(profile);
                    await _context.SaveChangesAsync();
                    return Ok("User Created Successfully");
                }
                else
                {
                    return BadRequest("Inputs does not meet requirements !");
                }

            }
            else
            {
                return BadRequest("User already exists !");
            }
        }

    }
}