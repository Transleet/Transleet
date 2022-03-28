using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Transleet.Services;

namespace Transleet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("plugin")]
    public class PluginController : Controller
    {
        private readonly IService<IPlugin> _pluginService;

        public PluginController(IService<IPlugin> pluginService)
        {
            _pluginService = pluginService;
        }

        [HttpGet("plugin")]
        public async Task<string> GetPlugin()
        {
            var plugin = (await _pluginService.GetAsync()).First();
            return plugin.GetPlugin();
        }

        public async Task<byte[][]> GetContent()
        {
            throw new NotImplementedException();
        }
    }
}
