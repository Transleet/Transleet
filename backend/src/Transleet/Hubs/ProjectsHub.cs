using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Hubs;

[AllowAnonymous]
public class ProjectsHub : Hub
{
    private readonly ILogger<ProjectsHub> _logger;
    private readonly IProjectService _service;

    public ProjectsHub(ILogger<ProjectsHub> logger, IProjectService service)
    {
        _logger = logger;
        _service = service;
    }

    public ChannelReader<ProjectNotification> Subscribe()
    {
        var channel = Channel.CreateUnbounded<ProjectNotification>();
        _service.Subscribe(async n =>
        {
            await channel.Writer.WriteAsync(n);
        });
        return channel.Reader;
    }
}
