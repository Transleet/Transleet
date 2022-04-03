﻿using System.Collections.Immutable;
using Orleans;
using Orleans.Runtime;

namespace Transleet.Grains
{
    public interface IProjectManagerGrain :  IGrainWithGuidKey

    {
        Task RegisterAsync(Guid itemKey);
        Task UnregisterAsync(Guid itemKey);

        Task<ImmutableArray<Guid>> GetAllAsync();
    }

    public class ProjectManagerGrain : Grain, IProjectManagerGrain
    {
        private readonly IPersistentState<State> _state;

        private Guid GrainKey => this.GetPrimaryKey();

        public ProjectManagerGrain(
            [PersistentState("State")] IPersistentState<State> state) => _state = state;

        public async Task RegisterAsync(Guid itemKey)
        {
            _state.State.Items.Add(itemKey);
            await _state.WriteStateAsync();
        }

        public async Task UnregisterAsync(Guid itemKey)
        {
            _state.State.Items.Remove(itemKey);
            await _state.WriteStateAsync();
        }

        public Task<ImmutableArray<Guid>> GetAllAsync() =>
            Task.FromResult(ImmutableArray.CreateRange(_state.State.Items));

        public class State
        {
            public HashSet<Guid> Items { get; set; } = new();
        }
    }
}