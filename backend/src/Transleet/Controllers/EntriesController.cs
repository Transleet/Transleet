using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Orleans;

using Swashbuckle.AspNetCore.Annotations;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Route("api/entries")]
public class EntriesController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public EntriesController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get entry.", OperationId = "GetEntry")]
    [HttpGet("{id:guid}")]
    public async Task<Entry?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<IEntryGrain>(id);
        var entry = await grain.GetAsync();
        return entry;
    }

    [SwaggerOperation(Summary = "Create entry.", OperationId = "CreateEntry")]
    [HttpPost]
    public async Task<IActionResult> PostAsync(Entry item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<IEntryGrain>(item.Key).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [SwaggerOperation(Summary = "Update entry.", OperationId = "UpdateEntry")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Entry item)
    {
        await _factory.GetGrain<IEntryGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [SwaggerOperation(Summary = "Delete entry.", OperationId = "DeleteEntry")]
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IEntryGrain>(id).ClearAsync();
}
