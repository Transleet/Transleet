using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs
{
    public class EntriesHub:Hub
    {
        private readonly IGrainFactory _grainFactory;

        public EntriesHub(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [Authorize]
        public async Task<Entry> Create(Entry entry)
        {
            entry.Key = Guid.NewGuid();
            var grain = _grainFactory.GetGrain<IEntryGrain>(entry.Key);
            await grain.SetAsync(entry);
            return entry;
        }

        public Task<Entry?> Get(Guid key)
        {
            var grain = _grainFactory.GetGrain<IEntryGrain>(key);
            return grain.GetAsync();
        }

        [Authorize]
        public async Task Update(Entry entry)
        {
            var grain = _grainFactory.GetGrain<IEntryGrain>(entry.Key);
            await grain.SetAsync(entry);
        }

        [Authorize]
        public async Task Delete(Guid key)
        {
            await _grainFactory.GetGrain<IEntryGrain>(key).ClearAsync();
        }
    }
}
