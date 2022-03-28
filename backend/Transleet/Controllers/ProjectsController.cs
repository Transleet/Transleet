using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;


[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly IService<Project> _service;

    public ProjectsController(IService<Project> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Project>> Get() =>
        await _service.GetAsync();


    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Project>> Get(string id)
    {
        var project = await _service.GetAsync(id);
        if (project is null)
        {
            return NotFound();
        }

        return project;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Project obj)
    {
        await _service.CreateAsync(obj);
        return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Project updated)
    {
        var project = await _service.GetAsync(id);
        if (project is null)
        {
            return NotFound();
        }

        updated.Id = project.Id;

        await _service.UpdateAsync(id, updated);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var project = await _service.GetAsync(id);
        if (project is null)
        {
            return NotFound();
        }

        await _service.RemoveAsync(project.Id);
        return NoContent();
    }
}