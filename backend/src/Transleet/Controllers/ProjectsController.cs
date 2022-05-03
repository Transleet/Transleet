using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using Transleet.Models;
using Transleet.Repositories;
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
    [HttpGet("{id:length(24)}",Name = "GetProjectById")]
    public Task<Project> GetProjectAsync(ObjectId id)
    {
        return _service.GetByIdAsync(id);
    }

    [HttpGet(Name = "GetAllProjects")]
    [AllowAnonymous]
    public IAsyncEnumerable<Project> GetProjectsAsync()
    {
        return _service.GetAllAsync();
    }

    [HttpPost(Name = "CreateProject")]
    public async Task<IActionResult> PostAsync(Project item)
    {
        await _service.AddAsync(item);
        return CreatedAtAction(nameof(GetProjectAsync), new { id = item.Id }, item);
    }

    [HttpPut(Name = "UpdateProject")]
    public async Task<IActionResult> UpdateAsync(Project item)
    {
        await _service.UpdateAsync(item);
        return Ok();
    }

    [HttpDelete("{id:length(24)}",Name = "DeleteProjectById")]
    public Task DeleteAsync(ObjectId id)
    {
        return _service.DeleteByIdAsync(id);
    }

}
