using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Transleet.Models;

namespace Transleet.Controllers;

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
    [HttpPost("token",Name = "GetToken"), Produces("application/json")]
    public async Task<IActionResult> GetTokenAsync([FromBody]LoginResource loginResource)
    {
        User? user;
        if (loginResource!.InputText.Contains('@'))
        {
            user = await _userManager.FindByEmailAsync(loginResource.InputText);
            if (user is null)
            {
                return BadRequest($"Can't find a user whose email is {loginResource.InputText}");
            }
        }
        else
        {
            user = await _userManager.FindByNameAsync(loginResource.InputText);
            if (user is null)
            {
                return BadRequest($"Can't find a user whose name is {loginResource.InputText}");
            }
        }

        if (await _userManager.CheckPasswordAsync(user, loginResource.Password))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtBearerOptions.CurrentValue.Issuer,
                Audience = _jwtBearerOptions.CurrentValue.Audience,
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(_jwtBearerOptions.CurrentValue.Key, SecurityAlgorithms.HmacSha256)
            };
            await _signInManager.SignInAsync(user, true);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { id = user.Id, token = tokenHandler.WriteToken(token) });
        }
        return BadRequest("Wrong password!");
    }

    public record LoginResource(string InputText, string Password);
}
