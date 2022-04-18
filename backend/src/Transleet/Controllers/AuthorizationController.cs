using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Transleet.Models;

namespace Transleet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/authorize")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<JwtOptions> _jwtBearerOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthorizationController> logger,
            IConfiguration configuration,
            IOptionsMonitor<JwtOptions> jwtBearerOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _jwtBearerOptions = jwtBearerOptions;
        }

        [AllowAnonymous]
        [HttpPost("token"), Produces("application/json")]
        public async Task<IActionResult> GetToken()
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
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = _jwtBearerOptions.CurrentValue.Issuer,
                    Audience = _jwtBearerOptions.CurrentValue.Audience,
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials =
                        new SigningCredentials(_jwtBearerOptions.CurrentValue.Key, SecurityAlgorithms.HmacSha256)
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
