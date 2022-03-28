using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[ApiController]
[Route("entries")]
public class EntriesController : ControllerBase
{
    private readonly IService<Entry> _service;
    private readonly ILogger<EntriesService> _logger;

    public EntriesController(IService<Entry> service,ILogger<EntriesService> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Entry>> Get() =>
        await _service.GetAsync();


    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Entry>> Get(string id)
    {
        var entry = await _service.GetAsync(id);
        if (entry is null)
        {
            return NotFound();
        }

        return entry;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Entry obj)
    {
        await _service.CreateAsync(obj);
        return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Entry updated)
    {
        var entry = await _service.GetAsync(id);
        if (entry is null)
        {
            return NotFound();
        }

        updated.Id = entry.Id;

        await _service.UpdateAsync(id, updated);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entry = await _service.GetAsync(id);
        if (entry is null)
        {
            return NotFound();
        }

        await _service.RemoveAsync(entry.Id);
        return NoContent();
    }
}