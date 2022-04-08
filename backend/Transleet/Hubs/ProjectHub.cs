using Microsoft.AspNetCore.Authentication.JwtBearer;
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

    public async IAsyncEnumerable<Project> GetAllProjects()
    {
        var keys = await _grainFactory.GetGrain<IKeySetGrain>(TransleetConstants.ProjectKeySet).GetAllAsync();
        foreach (var key in keys)
        {
            var grain = _grainFactory.GetGrain<IProjectGrain>(key);
            yield return await grain.GetAsync();
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Project> CreateProject(Project project)
    {
        project.Key = Guid.NewGuid();
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
        return project;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task UpdateProject(Project project)
    {
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task RemoveProject(Guid key)
    {
        await _grainFactory.GetGrain<IProjectGrain>(key).ClearAsync();
    }
}