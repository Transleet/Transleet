using Microsoft.AspNetCore.Mvc;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Route("api/translations")]
public class TranslationsController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public TranslationsController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("{id:guid}")]
    public async Task<Translation?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<ITranslationGrain>(id);
        var translation = await grain.GetAsync();
        return translation;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Translation item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<ITranslationGrain>(item.Key).SetAsync(item);
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Translation item)
    {
        await _factory.GetGrain<ITranslationGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<ITranslationGrain>(id).ClearAsync();
}
