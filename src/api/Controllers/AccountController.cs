#nullable enable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{

    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<AccountController> logger,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Register a new account",
        OperationId = "Register"
    )]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterResource model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user != null)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        user = new User { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return Ok(new
            {
                username = user.UserName,
                email = user.Email
            });
        }

        return BadRequest(result.Errors);
    }
}

public record RegisterResource(string? Username, string Email, string Password);
