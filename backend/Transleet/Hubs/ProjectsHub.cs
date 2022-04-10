using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Orleans;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs;

[AllowAnonymous]
public class ProjectsHub : Hub
{
    private readonly IGrainFactory _grainFactory;

    public ProjectsHub(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public Task<Project?> Get(Guid key)
    {
        var grain = _grainFactory.GetGrain<IProjectGrain>(key);
        return grain.GetAsync();
    }

    public async IAsyncEnumerable<Project?> GetAll()
    {
        var keys = await _grainFactory.GetKeySet(typeof(IProjectGrain).FullName).GetAllAsync();
        foreach (var key in keys)
        {
            var grain = _grainFactory.GetGrain<IProjectGrain>(key);
            yield return await grain.GetAsync();
        }
    }

    public async IAsyncEnumerable<Project?> GetTopTen()
    {
        var keys = (await _grainFactory.GetKeySet(typeof(IProjectGrain).FullName).GetAllAsync()).TakeLast(10);
        foreach (var key in keys)
        {
            var grain = _grainFactory.GetGrain<IProjectGrain>(key);
            yield return await grain.GetAsync();
        }
    }

    [Authorize]
    public async Task<Project> Create(Project project)
    {
        project.Key = Guid.NewGuid();
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
        return project;
    }

    [Authorize]
    public async Task Update(Project project)
    {
        var grain = _grainFactory.GetGrain<IProjectGrain>(project.Key);
        await grain.SetAsync(project);
    }

    [Authorize]
    public async Task Delete(Guid key)
    {
        await _grainFactory.GetGrain<IProjectGrain>(key).ClearAsync();
    }
}
