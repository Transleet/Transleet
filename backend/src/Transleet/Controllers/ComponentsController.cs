using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Orleans;

using Swashbuckle.AspNetCore.Annotations;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Authorize]
[Route("api/components")]
public class ComponentsController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public ComponentsController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get a component by its id.",
        OperationId = "GetComponent"
    )]
    public async Task<Component?> GetComponentAsync(Guid id)
    {
        var grain = _factory.GetGrain<IComponentGrain>(id);
        var component = await grain.GetAsync();
        return component;
    }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get all components.",
        OperationId = "GetComponents"
    )]
    public async IAsyncEnumerable<Component?> GetComponentsAsync()
    {
        var keys = await _factory.GetKeySet<IComponentGrain>().GetAllAsync();
        foreach (var key in keys)
        {
            yield return await _factory.GetGrain<IComponentGrain>(key).GetAsync();
        }
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new component.",
        OperationId = "CreateComponent"
    )]
    public async Task<IActionResult> PostAsync(Component item)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTimeOffset.Now;
        item.UpdatedAt = DateTimeOffset.Now;
        await _factory.GetGrain<IComponentGrain>(item.Id).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetComponentAsync), new { id = item.Id }, item);
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Update a component.",
        OperationId = "UpdateComponent"
    )]
    public async Task<IActionResult> UpdateAsync(Component item)
    {
        await _factory.GetGrain<IComponentGrain>(item.Id).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete a component by its id.",
        OperationId = "DeleteComponent"
    )]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IComponentGrain>(id).ClearAsync();
}
