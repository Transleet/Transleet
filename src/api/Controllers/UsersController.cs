using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILogger<UsersController> _logger;


    public UsersController(IUserService service, ILogger<UsersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "GetUserById")]
    public Task<User?> GetUserByIdAsync(Guid id)
    {
        return _service.GetByIdAsync(id);
    }

    [HttpGet(Name = "GetAllUsers")]
    [AllowAnonymous]
    public IAsyncEnumerable<User> GetAllUsersAsync()
    {
        return _service.GetAllAsync();
    }
}
