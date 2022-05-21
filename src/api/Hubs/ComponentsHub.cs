using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Transleet.Models;
using Transleet.Services;

namespace Transleet.Hubs;

[AllowAnonymous]
public class ComponentsHub : Hub
{
    private readonly ILogger<ComponentsHub> _logger;
    private readonly IComponentService _service;

    public ComponentsHub(ILogger<ComponentsHub> logger, IComponentService service)
    {
        _logger = logger;
        _service = service;
    }

    public ChannelReader<ComponentNotification> Subscribe()
    {
        var channel = Channel.CreateUnbounded<ComponentNotification>();
        _service.Subscribe(async n =>
        {
            await channel.Writer.WriteAsync(n);
        });
        return channel.Reader;
    }
}
