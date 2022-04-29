using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Orleans;

using Swashbuckle.AspNetCore.Annotations;

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

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get translation.", OperationId = "GetTranslation")]
    [HttpGet("{id:guid}")]
    public async Task<Translation?> GetAsync(Guid id)
    {
        var grain = _factory.GetGrain<ITranslationGrain>(id);
        var translation = await grain.GetAsync();
        return translation;
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Filter translations with locale.", OperationId = "FilterTranslationsWithLocale")]
    [HttpGet("{id:guid}")]
    public async IAsyncEnumerable<Translation?> GetAllWithLocaleAsync(Guid[] guids, Locale locale)
    {
        foreach (Guid guid in guids)
        {
            var grains = _factory.GetGrain<ITranslationGrain>(guid);
            var translation = await grains.GetAsync();
            if (!Equals(translation!.To.Locale, locale))
            {
                continue;
            }

            yield return translation;
        }
    }

    [SwaggerOperation(Summary = "Create translation.", OperationId = "CreateTranslation")]
    [HttpPost]
    public async Task<IActionResult> PostAsync(Translation item)
    {
        item.Key = Guid.NewGuid();
        await _factory.GetGrain<ITranslationGrain>(item.Key).SetAsync(item);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetAsync), new { id = item.Key }, item);
    }

    [SwaggerOperation(Summary = "Update translation.", OperationId = "UpdateTranslation")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Translation item)
    {
        await _factory.GetGrain<ITranslationGrain>(item.Key).SetAsync(item);
        return Ok();
    }

    [SwaggerOperation(Summary = "Delete translation.", OperationId = "DeleteTranslation")]
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id) =>
        _factory.GetGrain<ITranslationGrain>(id).ClearAsync();
}
