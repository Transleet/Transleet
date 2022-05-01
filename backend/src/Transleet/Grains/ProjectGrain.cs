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

        Task AddComponentAsync(Guid component);

        Task<bool> RemoveComponentAsync(Guid component);

    }

    public class ProjectGrain : Grain, IProjectGrain
    {
        private readonly ILogger<ProjectGrain> _logger;
        private readonly IPersistentState<ProjectItemState> _itemState;
        private int _componentCount;
        private bool _isLocked;

        public ProjectGrain(
            ILogger<ProjectGrain> logger,
            [PersistentState("ProjectItem", "Default")] IPersistentState<ProjectItemState> itemState
            )
        {
            _logger = logger;
            _itemState = itemState;
        }

        private static string GrainType => typeof(IProjectGrain).FullName!;
        private Guid GrainKey => this.GetPrimaryKey();

        public override Task OnActivateAsync()
        {
            _componentCount = _itemState.State.Item.Components.Count;
            return Task.CompletedTask;
        }

        public async Task SetAsync(Project item)
        {
            if (item.Id != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _itemState.State.Item = item;
            await _itemState.WriteStateAsync();
            await GrainFactory.GetKeySet(GrainType).AddAsync(item.Id);

            GetStreamProvider("SMS").GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(item.Id, NotificationOperation.CreatedOrUpdated))
                .Ignore();

        }

        public async Task ClearAsync()
        {
            if (_itemState.State.Item is null) return;

            var itemKey = _itemState.State.Item.Id;
            await GrainFactory.GetKeySet(GrainType).DeleteAsync(itemKey);
            await _itemState.ClearStateAsync();

            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(itemKey, NotificationOperation.Removed))
                .Ignore();

            DeactivateOnIdle();
        }

        public Task<Project?> GetAsync()
        {
            return Task.FromResult(_itemState.State.Item);
        }

        public async Task AddComponentAsync(Guid component)
        {
            _itemState.State.Item.Components.Add(component);
            _componentCount++;
            await _itemState.WriteStateAsync();
        }

        public async Task<bool> RemoveComponentAsync(Guid component)
        {
            bool isSuccessful = _itemState.State.Item.Components.Remove(component);
            if (isSuccessful)
            {
                _componentCount--;
                await _itemState.WriteStateAsync();
            }
            return isSuccessful;
        }

        public class ProjectItemState
        {
            public Project? Item { get; set; }
        }
    }
}
