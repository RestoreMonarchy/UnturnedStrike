using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxesController : ControllerBase
    {
        private readonly IBoxesRepository boxesRepository;

        public BoxesController(IBoxesRepository boxesRepository)
        {
            this.boxesRepository = boxesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if (User.IsInRole(RoleConstants.AdminRoleId))
            {
                return Ok(await boxesRepository.GetWeaponSkinBoxesAsync());
            }
            return Ok(await boxesRepository.GetBoxesAsync());   
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Box box)
        {
            return Ok(await boxesRepository.AddBoxAsync(box));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Box box)
        {
            await boxesRepository.UpdateBoxAsync(box);
            return Ok();
        }

        [HttpGet("weaponskin/{boxId}")]
        public async Task<IActionResult> GetWeaponSkinBoxAsync(int boxId)
        {
            return Ok(await boxesRepository.GetWeaponSkinBoxAsync(boxId));
        }
    }
}
