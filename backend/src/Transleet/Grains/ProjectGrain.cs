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
        public IPersistentState<ProjectItemState> ItemState { get; }
        public IPersistentState<ProjectComponentsState> ComponentsState { get; }

        public ProjectGrain(
            ILogger<ProjectGrain> logger,
            [PersistentState("ProjectItem", "Default")] IPersistentState<ProjectItemState> itemState,
            [PersistentState("ProjectComponents", "Default")] IPersistentState<ProjectComponentsState> componentsState
            )
        {
            _logger = logger;
            ItemState = itemState;
            ComponentsState = componentsState;
        }

        private static string GrainType => typeof(IProjectGrain).FullName!;
        private Guid GrainKey => this.GetPrimaryKey();

        public async Task SetAsync(Project item)
        {
            if (item.Id != GrainKey)
            {
                throw new InvalidOperationException();
            }

            ItemState.State.Item = item;
            await ItemState.WriteStateAsync();
            await GrainFactory.GetKeySet(GrainType).AddAsync(item.Id);

            GetStreamProvider("SMS").GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(item.Id, NotificationOperation.CreatedOrUpdated))
                .Ignore();

        }

        public async Task ClearAsync()
        {
            if (ItemState.State.Item is null) return;

            var itemKey = ItemState.State.Item.Id;
            await GrainFactory.GetKeySet(GrainType).DeleteAsync(itemKey);
            await ItemState.ClearStateAsync();

            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>()
                .OnNextAsync(new ProjectNotification(itemKey, NotificationOperation.Removed))
                .Ignore();

            DeactivateOnIdle();
        }

        public Task<Project?> GetAsync()
        {
            return Task.FromResult(ItemState.State.Item);
        }

        public async Task AddComponentAsync(Guid component)
        {
            ItemState.State.Item!.Components!.Add(component);
            ComponentsState.State.ComponentCount++;
            await ComponentsState.WriteStateAsync();
            await ItemState.WriteStateAsync();
        }

        public async Task<bool> RemoveComponentAsync(Guid component)
        {
            bool isSuccessful = ItemState.State.Item!.Components!.Remove(component);
            if (isSuccessful)
            {
                ComponentsState.State.ComponentCount--;
                await ComponentsState.WriteStateAsync();
                await ItemState.WriteStateAsync();
            }
            return isSuccessful;
        }

        public class ProjectItemState
        {
            public Project? Item { get; set; }
        }

        public class ProjectComponentsState
        {
            public int ComponentCount { get; set; }
        }
    }
}
