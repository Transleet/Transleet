using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs
{
    public class TranslationsHub:Hub
    {

        private readonly IGrainFactory _grainFactory;

        public TranslationsHub(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [Authorize]
        public async Task<Translation> Create(Translation translation)
        {
            translation.Key = Guid.NewGuid();
            var grain = _grainFactory.GetGrain<ITranslationGrain>(translation.Key);
            await grain.SetAsync(translation);
            return translation;
        }

        public Task<Translation?> Get(Guid key)
        {
            var grain = _grainFactory.GetGrain<ITranslationGrain>(key);
            return grain.GetAsync();
        }

        [Authorize]
        public async Task Update(Translation translation)
        {
            var grain = _grainFactory.GetGrain<ITranslationGrain>(translation.Key);
            await grain.SetAsync(translation);
        }

        [Authorize]
        public async Task Delete(Guid key)
        {
            await _grainFactory.GetGrain<ITranslationGrain>(key).ClearAsync();
        }
    }
}
