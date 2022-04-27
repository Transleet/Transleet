 using Microsoft.AspNetCore.Mvc;
using Orleans;
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

    [HttpGet("{id:guid}")]
    public async Task<Entry?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<IEntryGrain>(id);
        var entry = await grain.GetAsync();
        return entry;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Entry item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<IEntryGrain>(item.Key).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Entry item)
    {
        await _factory.GetGrain<IEntryGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IEntryGrain>(id).ClearAsync();
}
