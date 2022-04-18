using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using Orleans;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet.Hubs
{
    [Authorize]
    public class UsersHub : Hub
    {
        private readonly IGrainFactory _grainFactory;
        private readonly UserManager<User> _userManager;

        public UsersHub(IGrainFactory grainFactory, UserManager<User> userManager)
        {
            _grainFactory = grainFactory;
            _userManager = userManager;
        }

        public async IAsyncEnumerable<string> GetAllOnlineUsers()
        {
            var keySet = _grainFactory.GetKeySet<UserGrain>();
            foreach (var key in await keySet.GetAllAsync())
            {
                yield return key.ToString("D");
            }
        }

        public override async Task OnConnectedAsync()
        {
            var keySet = _grainFactory.GetKeySet<UserGrain>();
            var user = await _userManager.GetUserAsync(Context.User);
            if (user is not null)
            {
                await keySet.AddAsync(user.Id);
                await Clients.Others.SendAsync("OnUserConnected", user.Id.ToString("D"));
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var keySet = _grainFactory.GetKeySet<UserGrain>();
            var user = await _userManager.GetUserAsync(Context.User);
            if (user is not null)
            {
                await keySet.DeleteAsync(user.Id);
                await Clients.Others.SendAsync("OnUserDisconnected", user.Id.ToString("D"));
            }
        }
    }
}
