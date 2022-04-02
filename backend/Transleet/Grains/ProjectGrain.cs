using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public class ProjectGrain : Grain, IProjectGrain
    {
        private readonly ILogger<ProjectGrain> _logger;
        private readonly IPersistentState<State> _state;

        public ProjectGrain(ILogger<ProjectGrain> logger, [PersistentState("State")]IPersistentState<State> state)
        {
            _logger = logger;
            _state = state;
        }   

        private static string GrainType => nameof(ProjectGrain);
        private Guid GrainKey => this.GetPrimaryKey();

        public async Task SetAsync(Project item)
        {
            // ensure the key is consistent
            if (item.Id != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();
            await GrainFactory.GetGrain<IProjectManagerGrain>(item.OwnerId).RegisterAsync(item.Id);

            _logger.LogInformation(
                "{@GrainType} {@GrainKey} now contains {@Project}",
                GrainType, GrainKey, item);

            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>(item.OwnerId, nameof(IProjectGrain))
                .OnNextAsync(new ProjectNotification(item.Id, item))
                .Ignore();
        }

        public async Task ClearAsync()
        {
            // fast path for already cleared state
            if (_state.State.Item is null) return;

            // hold on to the keys
            var itemKey = _state.State.Item.Id;
            var ownerKey = _state.State.Item.OwnerId;

            // unregister from the registry
            await GrainFactory.GetGrain<IProjectManagerGrain>(ownerKey)
                .UnregisterAsync(itemKey);

            // clear the state
            await _state.ClearStateAsync();

            // for sample debugging
            _logger.LogInformation(
                "{@GrainType} {@GrainKey} is now cleared",
                GrainType, GrainKey);

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>(ownerKey, nameof(IProjectGrain))
                .OnNextAsync(new ProjectNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Project?> GetAsync()
        {
            return Task.FromResult(_state.State.Item);
        }

        public class State
        {
            public Project? Item { get; set; }
        }
    }
}
