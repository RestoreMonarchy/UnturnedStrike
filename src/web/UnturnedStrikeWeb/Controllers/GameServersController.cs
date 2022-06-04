using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeWeb.Services;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameServersController : ControllerBase
    {
        private readonly ServersStatusService serversStatusService;

        public GameServersController(ServersStatusService serversStatusService)
        {
            this.serversStatusService = serversStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<GameServer> gameServers = await serversStatusService.GetGameServersFromCacheAsync();
            return Ok(gameServers);
        }
    }
}
