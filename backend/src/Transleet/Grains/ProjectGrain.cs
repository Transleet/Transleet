using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IProjectGrain : IGrainWithGuidKey
    {
        Task SetAsync(Project item);

        Task ClearAsync();

        Task<Project?> GetAsync();
    }

    public class ProjectGrain : Grain, IProjectGrain
    {
        private readonly ILogger<ProjectGrain> _logger;
        private readonly IPersistentState<ProjectGrainState> _state;

        public ProjectGrain(ILogger<ProjectGrain> logger, [PersistentState(nameof(ProjectGrainState), "Default")] IPersistentState<ProjectGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        private static string GrainType => typeof(IProjectGrain).FullName;
        private Guid GrainKey => this.GetPrimaryKey();

        public async Task SetAsync(Project item)
        {
            // ensure the key is consistent
            if (item.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();
            await GrainFactory.GetKeySet(GrainType).AddAsync(item.Key);

            GetStreamProvider("SMS").GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(item.Key, item))
                .Ignore();

        }

        public async Task ClearAsync()
        {
            // fast path for already cleared state
            if (_state.State.Item is null) return;

            // hold on to the keys
            var itemKey = _state.State.Item.Key;
            await GrainFactory.GetKeySet(GrainType).DeleteAsync(itemKey);
            // clear the state
            await _state.ClearStateAsync();

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Project?> GetAsync()
        {
            return Task.FromResult(_state.State.Item);
        }

        public class ProjectGrainState
        {
            public Project? Item { get; set; }
        }
    }
}
