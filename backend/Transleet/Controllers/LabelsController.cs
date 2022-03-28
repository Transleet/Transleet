using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[ApiController]
[Route("labels")]
public class LabelsController : ControllerBase
{
    private readonly IService<Label> _service;

    public LabelsController(IService<Label> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Label>> Get() =>
        await _service.GetAsync();


    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Label>> Get(string id)
    {
        var label = await _service.GetAsync(id);
        if (label is null)
        {
            return NotFound();
        }

        return label;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Label obj)
    {
        await _service.CreateAsync(obj);
        return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Label updated)
    {
        var label = await _service.GetAsync(id);
        if (label is null)
        {
            return NotFound();
        }

        updated.Id = label.Id;

        await _service.UpdateAsync(id, updated);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var label = await _service.GetAsync(id);
        if (label is null)
        {
            return NotFound();
        }

        await _service.RemoveAsync(label.Id);
        return NoContent();
    }
}