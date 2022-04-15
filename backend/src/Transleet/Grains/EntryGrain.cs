// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    interface IEntryGrain : IGrainWithGuidKey
    {
        Task SetAsync(Entry item);

        Task ClearAsync();

        Task<Entry?> GetAsync();
    }
    public class EntryGrain : Grain, IEntryGrain
    {
        private readonly ILogger<EntryGrain> _logger;
        private readonly IPersistentState<EntryGrainState> _state;
        private static readonly Guid s_keySetId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(IEntryGrain))));
        private static readonly Guid s_streamId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(IEntryGrain))));

        public EntryGrain(ILogger<EntryGrain> logger, [PersistentState(nameof(EntryGrainState), "Default")] IPersistentState<EntryGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        private static string GrainType => nameof(EntryGrain);
        private Guid GrainKey => this.GetPrimaryKey();
        public Task<Guid> GetKeySetId() => Task.FromResult(s_keySetId);

        public Task<Guid> GetStreamId() => Task.FromResult(s_streamId);

        public async Task SetAsync(Entry item)
        {
            // ensure the key is consistent
            if (item.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();

            GetStreamProvider("SMS")
                .GetStream<EntryNotification>()
                .OnNextAsync(new EntryNotification(item.Key, item))
                .Ignore();
        }

        public async Task ClearAsync()
        {
            // fast path for already cleared state
            if (_state.State.Item is null) return;

            // hold on to the keys
            var itemKey = _state.State.Item.Key;
            // clear the state
            await _state.ClearStateAsync();

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<EntryNotification>()
                .OnNextAsync(new EntryNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Entry?> GetAsync()
        {
            return Task.FromResult(_state.State.Item);
        }

        public class EntryGrainState
        {
            public Entry? Item { get; set; }
        }
    }
}
