using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Orleans;

using Swashbuckle.AspNetCore.Annotations;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Route("api/components")]
public class ComponentsController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public ComponentsController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get component.", OperationId = "GetComponent")]
    [HttpGet("{id:guid}")]
    public async Task<Component?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<IComponentGrain>(id);
        var component = await grain.GetAsync();
        return component;
    }

    [SwaggerOperation(Summary = "Create component.", OperationId = "CreateComponent")]
    [HttpPost]
    public async Task<IActionResult> PostAsync(Component item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<IComponentGrain>(item.Key).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [SwaggerOperation(Summary = "Update component.", OperationId = "UpdateComponent")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Component item)
    {
        await _factory.GetGrain<IComponentGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [SwaggerOperation(Summary = "Delete component.", OperationId = "DeleteComponent")]
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IComponentGrain>(id).ClearAsync();
}
