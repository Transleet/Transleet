using System.Collections.Immutable;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Transleet.Grains
{
    public interface IKeySetGrain : IGrainWithGuidKey
    {
        Task<bool> AddAsync(Guid key);

        Task<bool> DeleteAsync(Guid key);

        [AlwaysInterleave]
        Task<bool> ExistAsync(Guid key);

        [AlwaysInterleave]
        Task<ImmutableArray<Guid>> GetAllAsync();
    }
    public class KeySetGrain : Grain, IKeySetGrain
    {
        private readonly IPersistentState<KeySetGrainState> _state;

        public KeySetGrain(
            [PersistentState(nameof(KeySetGrainState), "Default")] IPersistentState<KeySetGrainState> state)
        {
            _state = state;
        }

        public async Task<bool> AddAsync(Guid key)
        {
            if (_state.State.KeySet.Add(key))
            {
                await _state.WriteStateAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid key)
        {
            if (_state.State.KeySet.Remove(key))
            {
                await _state.WriteStateAsync();
                return true;
            }
            return false;
        }

        public Task<bool> ExistAsync(Guid key)
        {
            if (_state.State.KeySet.Contains(key))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<ImmutableArray<Guid>> GetAllAsync()
        {
            return Task.FromResult(_state.State.KeySet.ToImmutableArray());
        }
    }

    public class KeySetGrainState
    {
        public HashSet<Guid> KeySet { get; set; } = new();
    }
}
