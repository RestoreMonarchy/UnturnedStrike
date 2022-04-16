using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameSummariesController : ControllerBase
    {
        private readonly IGameSummariesRepository gameSummariesRepository;

        public GameSummariesController(IGameSummariesRepository gameSummariesRepository)
        {
            this.gameSummariesRepository = gameSummariesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GameSummary gameSummary)
        {
            return Ok(await gameSummariesRepository.CreateGameSummary(gameSummary));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(GameSummary gameSummary)
        {
            await gameSummariesRepository.UpdateGameSummary(gameSummary);
            return Ok();
        }
    }
}
