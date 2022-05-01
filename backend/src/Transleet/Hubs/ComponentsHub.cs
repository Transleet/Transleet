// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs
{
    public class ComponentsHub:Hub
    {
        private readonly IGrainFactory _grainFactory;

        public ComponentsHub(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [Authorize]
        public async Task<Component> Create(Component component)
        {
            component.Id = Guid.NewGuid();
            var grain = _grainFactory.GetGrain<IComponentGrain>(component.Id);
            await grain.SetAsync(component);
            return component;
        }

        public Task<Component?> Get(Guid key)
        {
            var grain = _grainFactory.GetGrain<IComponentGrain>(key);
            return grain.GetAsync();
        }

        [Authorize]
        public async Task Update(Component component)
        {
            var grain = _grainFactory.GetGrain<IComponentGrain>(component.Id);
            await grain.SetAsync(component);
        }

        [Authorize]
        public async Task Delete(Guid key)
        {
            await _grainFactory.GetGrain<IComponentGrain>(key).ClearAsync();
        }
    }
}
