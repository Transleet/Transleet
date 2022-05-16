using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[ApiController]
[Authorize]
[Route("api/components")]
public class ComponentsController : ControllerBase
{
    private readonly IComponentService _service;

    public ComponentsController(IComponentService service) => _service = service;

    [AllowAnonymous]
    [HttpGet("{id:guid}",Name = "GetComponentById")]
    public Task<Component?> GetComponentAsync(Guid id)
    {
        return _service.GetByIdAsync(id);
    }

    [HttpPost(Name = "CreateComponent")]
    public async Task<IActionResult> PostAsync(Component item)
    {
        await _service.AddAsync(item);
        return CreatedAtAction(nameof(GetComponentAsync), new { id = item.Id }, item);
    }

    [HttpPut(Name = "UpdateComponent")]
    public Task UpdateAsync(Component item)
    {
        return _service.UpdateAsync(item);
    }

    [HttpDelete("{id:guid}", Name = "DeleteComponentById")]
    public Task DeleteAsync(Guid id)
    {
        return _service.DeleteByIdAsync(id);
    }
}
