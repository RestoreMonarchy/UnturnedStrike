using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using UnturnedStrikeAPI;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameServersController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;

        public GameServersController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(memoryCache.Get<IEnumerable<GameServer>>("Servers"));
        }
    }
}
