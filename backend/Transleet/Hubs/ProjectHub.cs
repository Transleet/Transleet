﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs;

public class ProjectHub : Hub
{
    private readonly IGrainFactory _grainFactory;

    public ProjectHub(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async IAsyncEnumerable<Project?> GetAll()
    {
        var keys = await _grainFactory.GetGrain<IKeySetGrain>(TransleetConstants.ProjectKeySet).GetAllAsync();
        foreach (var key in keys)
        {
            var grain = _grainFactory.GetGrain<IProjectGrain>(key);
            yield return await grain.GetAsync();
        }
    }
    
    public Task<Project?> Get(Guid key)
    {
        var grain = _grainFactory.GetGrain<IProjectGrain>(key);
        return grain.GetAsync();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Project> Create(Project project)
    {
        project.Key = Guid.NewGuid();
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
        return project;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Update(Project project)
    {
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Delete(Guid key)
    {
        await _grainFactory.GetGrain<IProjectGrain>(key).ClearAsync();
    }
}