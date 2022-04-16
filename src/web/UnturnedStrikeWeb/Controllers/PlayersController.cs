using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeDatabaseProvider.Repositories;
using UnturnedStrikeWeb.Filters;
using UnturnedStrikeWeb.Services;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> logger;
        private readonly IPlayersRepository playersRepository;
        private readonly IPlayerService playerService;

        public PlayersController(ILogger<PlayersController> logger, IPlayersRepository playersRepository, IPlayerService playerService)
        {
            this.logger = logger;
            this.playersRepository = playersRepository;
            this.playerService = playerService;
        }

        [ApiKeyAuth]
        [HttpGet("{steamId}")]
        public async Task<IActionResult> GetPlayerAsync([FromRoute] string steamId)
        {
            return Ok(await playersRepository.GetPlayerAsync(steamId));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await playersRepository.GetPlayersAsync());
        }

        [ApiKeyAuth]
        [HttpPut]
        public async Task<IActionResult> PutAsync(Player player)
        {
            return Ok(await playerService.UpdatePlayerAsync(player));
        }

        [ApiKeyAuth]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Player player)
        {
            await playersRepository.CreatePlayerAsync(player.Id);
            return Ok(await playerService.UpdatePlayerAsync(player));
        }
    }
}
