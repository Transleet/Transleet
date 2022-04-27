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

        Task AddComponentAsync(ComponentGrain component);

        Task<bool> RemoveComponentAsync(ComponentGrain component);

        Task<bool> UpdateComponentAsync(ComponentGrain component);
    }

    public class ProjectGrain : Grain, IProjectGrain
    {
        private readonly ILogger<ProjectGrain> _logger;
        private readonly IPersistentState<ProjectGrainState> _itemState;
        private readonly IPersistentState<ProjectStatisticsState> _statisticsState;

        public ProjectGrain(
            ILogger<ProjectGrain> logger,
            [PersistentState(nameof(ProjectGrainState), "Default")] IPersistentState<ProjectGrainState> itemState,
            [PersistentState(nameof(ProjectStatisticsState), "Default")] IPersistentState<ProjectStatisticsState> statisticsState
            )
        {
            _logger = logger;
            _itemState = itemState;
            _statisticsState = statisticsState;
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

            _itemState.State.Item = item;
            await _itemState.WriteStateAsync();
            await GrainFactory.GetKeySet(GrainType).AddAsync(item.Key);

            GetStreamProvider("SMS").GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(item.Key, NotificationOperation.CreatedOrUpdated, item))
                .Ignore();

        }

        public async Task ClearAsync()
        {
            // fast path for already cleared state
            if (_itemState.State.Item is null) return;

            // hold on to the keys
            var itemKey = _itemState.State.Item.Key;
            await GrainFactory.GetKeySet(GrainType).DeleteAsync(itemKey);
            // clear the state
            await _itemState.ClearStateAsync();

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(itemKey, NotificationOperation.Removed, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public Task<Project?> GetAsync()
        {
            return Task.FromResult(_itemState.State.Item);
        }

        public async Task AddComponentAsync(ComponentGrain component)
        {
            _itemState.State.Components.Add(component);
            _statisticsState.State.ComponentCount++;
            await _itemState.WriteStateAsync();
            await _statisticsState.WriteStateAsync();
        }

        public async Task<bool> RemoveComponentAsync(ComponentGrain component)
        {
            bool isSuccessful = _itemState.State.Components.Remove(component);
            if (isSuccessful)
            {
                _statisticsState.State.ComponentCount--;
                await _itemState.WriteStateAsync();
                await _statisticsState.WriteStateAsync();
            }
            return isSuccessful;
        }

        public async Task<bool> UpdateComponentAsync(ComponentGrain component)
        {
            bool isSuccessful = _itemState.State.Components.Remove(component);
            if (isSuccessful)
            {
                _itemState.State.Components.Add(component);
                await _itemState.WriteStateAsync();
            }
            return isSuccessful;
        }

        public class ProjectGrainState
        {
            public HashSet<ComponentGrain> Components { get; set; } = new();
            public Project? Item { get; set; }
        }

        public class ProjectStatisticsState
        {
            public int ComponentCount { get; set; }
        }
    }
}
