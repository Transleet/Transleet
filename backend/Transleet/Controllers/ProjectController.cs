using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;


[ApiController]
[Route("projects")]
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

    [HttpGet()]
    public async IAsyncEnumerable<Project?> GetAllAsync()
    {
        var keys = await _factory.GetGrain<IKeySetGrain>(TransleetConstants.ProjectKeySet).GetAllAsync();
        foreach (var key in keys)
        {
            yield return await _factory.GetGrain<IProjectGrain>(key).GetAsync();
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Project item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<IProjectGrain>(item.Key).SetAsync(item);
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Project item)
    {
        await _factory.GetGrain<IProjectGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IProjectGrain>(id).ClearAsync();
}