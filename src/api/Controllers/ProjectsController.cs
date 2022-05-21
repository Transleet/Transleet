using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    private readonly ILogger<ProjectsController> _logger;
    

    public ProjectsController(IProjectService service, ILogger<ProjectsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "GetProjectById")]
    public Task<Project?> GetProjectByIdAsync(Guid id)
    {
        return _service.GetByIdAsync(id);
    }

    [HttpGet(Name = "GetAllProjects")]
    [AllowAnonymous]
    public IAsyncEnumerable<Project> GetAllProjectsAsync()
    {
        return _service.GetAllAsync();
    }

    [HttpPost(Name = "CreateProject")]
    public async Task<IActionResult> CreateProjectAsync(Project item)
    {
        await _service.AddAsync(item);
        return CreatedAtAction(nameof(GetProjectByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut(Name = "UpdateProject")]
    public async Task<IActionResult> UpdateProjectAsync(Project item)
    {
        await _service.UpdateAsync(item);
        return Ok();
    }

    [HttpDelete("{id:guid}", Name = "DeleteProjectById")]
    public Task DeleteProjectByIdAsync(Guid id)
    {
        return _service.DeleteByIdAsync(id);
    }

}
