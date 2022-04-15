using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IComponentGrain : IGrainWithGuidKey
    {
        Task SetAsync(Component item);

        Task ClearAsync();

        Task<Component?> GetAsync();
    }

    public class ComponentGrain : Grain, IComponentGrain
    {
        private static readonly Guid s_streamId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(IComponentGrain))));
        private readonly ILogger<ComponentGrain> _logger;
        private readonly IPersistentState<ComponentGrainState> _state;


        public ComponentGrain(ILogger<ComponentGrain> logger, [PersistentState(nameof(ComponentGrainState), "Default")] IPersistentState<ComponentGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        private static string GrainType => nameof(ComponentGrain);
        private Guid GrainKey => this.GetPrimaryKey();

        public Task<Guid> GetStreamId() => Task.FromResult(s_streamId);


        public async Task SetAsync(Component item)
        {
            // ensure the key is consistent
            if (item.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();

            GetStreamProvider("SMS")
                .GetStream<ComponentNotification>(GrainType)
                .OnNextAsync(new ComponentNotification(item.Key, item))
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
                .GetStream<ComponentNotification>(GrainType)
                .OnNextAsync(new ComponentNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Component?> GetAsync()
        {
            return Task.FromResult(_state.State.Item);
        }

        public class ComponentGrainState
        {
            public Component? Item { get; set; }
        }
    }
}
