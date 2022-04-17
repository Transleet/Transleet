using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Transactions;

namespace Transleet.Grains
{
    public interface ILookupGrain<TKey> : IGrainWithGuidKey where TKey : notnull
    {
        Task<bool> AddOrUpdateAsync(TKey value, Guid grainKey);

        Task DeleteAsync(TKey value);

        Task DeleteIfMatchAsync(TKey value, Guid grainKey);

        [AlwaysInterleave]
        Task<Guid?> FindAsync(TKey value);

        [AlwaysInterleave]
        Task<IReadOnlyDictionary<TKey, Guid>> GetAllAsync();
    }

    public class LookupGrain<TKey> : Grain, ILookupGrain<TKey> where TKey : notnull
    {
        private readonly IPersistentState<LookupGrainState> _state;

        public LookupGrain(
            [PersistentState(nameof(LookupGrainState), "Default")] IPersistentState<LookupGrainState> state)
        {
            _state = state;
        }

        public async Task<bool> AddOrUpdateAsync(TKey value, Guid grainKey)
        {
            if (_state.State.Index.ContainsKey(value))
                return false;

            _state.State.Index[value] = grainKey;
            await _state.WriteStateAsync();
            return true;
        }

        public Task DeleteAsync(TKey value)
        {
            if (_state.State.Index.Remove(value))
                return _state.WriteStateAsync();

            return Task.CompletedTask;
        }

        public Task DeleteIfMatchAsync(TKey value, Guid grainKey)
        {
            if (_state.State.Index.ContainsKey(value) && _state.State.Index[value] == grainKey)
            {
                _state.State.Index.Remove(value);
                return _state.WriteStateAsync();
            }
            return Task.CompletedTask;
        }

        public Task<Guid?> FindAsync(TKey value)
        {
            if (_state.State.Index.ContainsKey(value))
                return Task.FromResult<Guid?>(_state.State.Index[value]);

            return Task.FromResult<Guid?>(default);
        }

        public Task<IReadOnlyDictionary<TKey, Guid>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyDictionary<TKey, Guid>>(_state.State.Index);
        }

        public class LookupGrainState
        {
            public Dictionary<TKey, Guid> Index { get; set; } = new();
        }
    }

    
}
