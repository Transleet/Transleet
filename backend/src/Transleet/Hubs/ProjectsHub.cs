using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Orleans;
using Orleans.Services;
using Orleans.Streams;
using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs;

[AllowAnonymous]
public class ProjectsHub : Hub
{
    private readonly IClusterClient _clusterClient;
    private readonly ILogger<ProjectsHub> _logger;

    public ProjectsHub(IClusterClient clusterClient, ILogger<ProjectsHub> logger)
    {
        _clusterClient = clusterClient;
        _logger = logger;
    }

    public async Task SubscribeProjectNotification()
    {
        var stream = _clusterClient.GetStreamProvider("SMS").GetStream<ProjectNotification>();
        _logger.LogInformation("Projects Subscribed.");
        await stream.SubscribeAsync(onNextAsync: async (item, token) =>
        {
            _logger.LogInformation("Project Changed. {ProjectId}",item.Id);
            await Clients.All.SendAsync("ProjectUpdated", item);
        });


    }
}
