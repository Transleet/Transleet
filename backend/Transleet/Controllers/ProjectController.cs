using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;


[ApiController]
[Route("project")]
public class ProjectController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public ProjectController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("{id:guid}")]
    public async Task<Project?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<IProjectGrain>(id);
        var project = await grain.GetAsync();
        return project;
    }

    [HttpGet("list/{ownerId}")]
    public async IAsyncEnumerable<Project?> GetAllAsync(Guid ownerId)
    {
        // Get all item keys for this owner.
        var keys =
            await _factory.GetGrain<IProjectManagerGrain>(ownerId)
                .GetAllAsync();

        // Fast path for empty owner.
        if (keys.Length is 0) yield break;

        // Fan out and get all individual items in parallel.
        // Issue all requests at the same time.
        var tasks =
            keys.Select(key => _factory.GetGrain<IProjectGrain>(key).GetAsync())
                .ToList();

        // Compose the result as requests complete
        for (var i = 0; i < keys.Length; ++i)
        {
            var item = await tasks[i];
            if (item is null) continue;
            yield return item;
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Project item)
    {
        item.Id = Guid.NewGuid();
        await _factory.GetGrain<IProjectGrain>(item.Id).SetAsync(item);
        return CreatedAtAction(nameof(GetAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Project item)
    {
        await _factory.GetGrain<IProjectGrain>(item.Id).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IProjectGrain>(id).ClearAsync();
}