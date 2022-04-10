using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IProjectGrain : IGrainWithGuidKey,IGrainWithKeySetId,IGrainWithStreamId
    {
        Task SetAsync(Project item);

        Task ClearAsync();

        Task<Project?> GetAsync();
    }

    public class ProjectGrain : Grain, IProjectGrain
    {
        private readonly ILogger<ProjectGrain> _logger;
        private readonly IPersistentState<ProjectGrainState> _state;
        private static readonly Guid s_keySetId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(IProjectGrain))));
        private static readonly Guid s_streamId = new(MD5.HashData(Encoding.UTF8.GetBytes(nameof(IProjectGrain))));

        public ProjectGrain(ILogger<ProjectGrain> logger, [PersistentState(nameof(ProjectGrainState), "Default")] IPersistentState<ProjectGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        private static string GrainType => nameof(ProjectGrain);
        private Guid GrainKey => this.GetPrimaryKey();
        public Task<Guid> GetKeySetId() => Task.FromResult(s_keySetId);

        public Task<Guid> GetStreamId() => Task.FromResult(s_streamId);

        public async Task SetAsync(Project item)
        {
            // ensure the key is consistent
            if (item.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            _state.State.Item = item;
            await _state.WriteStateAsync();
            await GrainFactory.GetGrain<IKeySetGrain>(TransleetConstants.ProjectKeySet).AddAsync(item.Key);

            GetStreamProvider("SMS")
                .GetStream<ProjectNotification,IProjectGrain>(this)
                .OnNextAsync(new ProjectNotification(item.Key, item))
                .Ignore();
        }

        public async Task ClearAsync()
        {
            // fast path for already cleared state
            if (_state.State.Item is null) return;

            // hold on to the keys
            var itemKey = _state.State.Item.Key;
            await GrainFactory.GetGrain<IKeySetGrain>(TransleetConstants.ProjectKeySet).DeleteAsync(itemKey);
            // clear the state
            await _state.ClearStateAsync();

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<ProjectNotification>(TransleetConstants.ProjectKeySet, nameof(IProjectGrain))
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