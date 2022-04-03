using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Transleet.Models;

namespace Transleet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("authorize")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthorizationController> logger,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var resource = await HttpContext.Request.ReadFromJsonAsync<LoginResource>();
            User? user;
            if (resource.InputText.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(resource.InputText);
                if (user is null)
                {
                    return BadRequest($"Can't find a user whose email is {resource.InputText}");
                }
            }
            else
            {
                user = await _userManager.FindByNameAsync(resource.InputText);
                if (user is null)
                {
                    return BadRequest($"Can't find a user whose name is {resource.InputText}");
                }
            }
            
            if (await _userManager.CheckPasswordAsync(user, resource.Password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = _configuration["Authentication:JwtBearer:Issuer"],
                    Audience = _configuration["Authentication:JwtBearer:Audience"],
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                await _signInManager.SignInAsync(user,true);
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
            return BadRequest("Wrong password!");
        }

        private record LoginResource(string InputText, string Password);
    }
}
