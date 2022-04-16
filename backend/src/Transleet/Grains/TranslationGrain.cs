﻿using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface ITranslationGrain : IGrainWithGuidKey
    {
        Task SetAsync(Translation item);

        Task ClearAsync();

        Task<Translation?> GetAsync();
    }
    public class TranslationGrain:Grain,ITranslationGrain
    {
        private readonly ILogger<TranslationGrain> _logger;
        private readonly IPersistentState<TranslationGrainState> _state;
        private static readonly Guid s_streamId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(ITranslationGrain))));

        public TranslationGrain(ILogger<TranslationGrain> logger, [PersistentState(nameof(TranslationGrainState), "Default")] IPersistentState<TranslationGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        private static string GrainType => typeof(ITranslationGrain).FullName;
        private Guid GrainKey => this.GetPrimaryKey();

        public Task<Guid> GetStreamId() => Task.FromResult(s_streamId);

        public async Task SetAsync(Translation item)
        {
            // ensure the key is consistent
            if (item.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();

            GetStreamProvider("SMS")
                .GetStream<TranslationNotification>()
                .OnNextAsync(new TranslationNotification(item.Key, item))
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
                .GetStream<TranslationNotification>()
                .OnNextAsync(new TranslationNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Translation?> GetAsync()
        {
            return Task.FromResult(_state.State.Item);
        }

        public class TranslationGrainState
        {
            public Translation? Item { get; set; }
        }
    }
}