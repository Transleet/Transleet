using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Transactions;

namespace Transleet.Grains
{
    public interface ILookupGrain : IGrainWithGuidKey
    {
        Task<bool> AddOrUpdateAsync(string value, Guid grainKey);

        Task DeleteAsync(string value);

        Task DeleteIfMatchAsync(string value, Guid grainKey);

        [AlwaysInterleave]
        Task<Guid?> FindAsync(string value);

        [AlwaysInterleave]
        Task<IReadOnlyDictionary<string, Guid>> GetAllAsync();
    }

    public class LookupGrain : Grain, ILookupGrain
    {
        private readonly IPersistentState<LookupGrainState> _state;

        public LookupGrain(
            [PersistentState(nameof(LookupGrainState), "Default")] IPersistentState<LookupGrainState> state)
        {
            _state = state;
        }

        public async Task<bool> AddOrUpdateAsync(string value, Guid grainKey)
        {
            if (_state.State.Index.ContainsKey(value))
                return false;

            _state.State.Index[value] = grainKey;
            await _state.WriteStateAsync();
            return true;
        }

        public Task DeleteAsync(string value)
        {
            if (_state.State.Index.Remove(value))
                return _state.WriteStateAsync();

            return Task.CompletedTask;
        }

        public Task DeleteIfMatchAsync(string value, Guid grainKey)
        {
            if (_state.State.Index.ContainsKey(value) && _state.State.Index[value] == grainKey)
            {
                _state.State.Index.Remove(value);
                return _state.WriteStateAsync();
            }
            return Task.CompletedTask;
        }

        public Task<Guid?> FindAsync(string value)
        {
            if (_state.State.Index.ContainsKey(value))
                return Task.FromResult<Guid?>(_state.State.Index[value]);

            return Task.FromResult<Guid?>(default);
        }

        public Task<IReadOnlyDictionary<string, Guid>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyDictionary<string, Guid>>(_state.State.Index);
        }
    }

    public class LookupGrainState
    {
        public Dictionary<string, Guid> Index { get; set; } = new();
    }
}