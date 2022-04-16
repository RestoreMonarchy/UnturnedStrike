using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Enumerators;
using UnturnedStrikeAPI.Params;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerStatisticsController : ControllerBase
    {
        private readonly IPlayerStatisticsRepository playerStatisticsRepository;

        public PlayerStatisticsController(IPlayerStatisticsRepository playerStatisticsRepository)
        {
            this.playerStatisticsRepository = playerStatisticsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetTopPlayersAsync([FromBody] PlayerStatisticsParams playerStatisticsParams)
        {
            return Ok(await playerStatisticsRepository.GetTopPlayersAsync(playerStatisticsParams));
        }


        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayerAsync(string playerId)
        {
            return Ok(await playerStatisticsRepository.GetPlayerStatisticsAsync(playerId));
        }
    }
}
