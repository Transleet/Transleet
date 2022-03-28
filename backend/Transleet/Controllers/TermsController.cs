using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Controllers;

[Authorize]
[ApiController]
[Route("terms")]
public class TermsController : ControllerBase
{
    private readonly IService<Term> _service;

    public TermsController(IService<Term> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Term>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Term>> Get(string id)
    {
        var vocabulary = await _service.GetAsync(id);
        if (vocabulary is null)
        {
            return NotFound();
        }

        return vocabulary;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Term obj)
    {
        await _service.CreateAsync(obj);
        return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Term updated)
    {
        var vocabulary = await _service.GetAsync(id);
        if (vocabulary is null)
        {
            return NotFound();
        }

        updated.Id = vocabulary.Id;

        await _service.UpdateAsync(id, updated);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var vocabulary = await _service.GetAsync(id);
        if (vocabulary is null)
        {
            return NotFound();
        }

        await _service.RemoveAsync(vocabulary.Id);
        return NoContent();
    }
}