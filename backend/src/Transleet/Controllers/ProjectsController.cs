using Microsoft.AspNetCore.Mvc;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public ProjectsController(IGrainFactory factory)
    {
        _factory = factory;
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get a project by its id.",
        OperationId = "GetProject"
    )]
    public async Task<Project?> GetProjectAsync(Guid id)
    {
        var grain = _factory.GetGrain<IProjectGrain>(id);
        var project = await grain.GetAsync();
        return project;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all projects.",
        OperationId = "GetProjects"
    )]
    public async IAsyncEnumerable<Project?> GetProjectsAsync()
    {
        var keys = await _factory.GetKeySet<IProjectGrain>().GetAllAsync();
        foreach (var key in keys)
        {
            yield return await _factory.GetGrain<IProjectGrain>(key).GetAsync();
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new project.",
        OperationId = "CreateProject"
    )]
    public async Task<IActionResult> PostAsync(Project item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<IProjectGrain>(item.Key).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetProjectAsync), new { id = item.Key }, item);
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Update a project.",
        OperationId = "UpdateProject"
    )]
    public async Task<IActionResult> UpdateAsync(Project item)
    {
        await _factory.GetGrain<IProjectGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete a project by its id.",
        OperationId = "DeleteProject"
    )]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<IProjectGrain>(id).ClearAsync();
}
