using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeDatabaseProvider.Repositories;
using UnturnedStrikeWeb.Filters;

namespace UnturnedStrikeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponSkinsController : ControllerBase
    {
        private readonly IWeaponSkinsRepository weaponSkinsRepository;

        public WeaponSkinsController(IWeaponSkinsRepository weaponSkinsRepository)
        {
            this.weaponSkinsRepository = weaponSkinsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await weaponSkinsRepository.GetWeaponSkinsAsync());
        }

        [HttpGet("{weaponSkinId}")]
        public async Task<IActionResult> GetWeaponSkinAsync(int weaponSkinId)
        {
            return Ok(await weaponSkinsRepository.GetWeaponSkinAsync(weaponSkinId));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] WeaponSkin weaponSkin)
        {
            return Ok(await weaponSkinsRepository.AddWeaponSkinAsync(weaponSkin));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] WeaponSkin weaponSkin)
        {
            await weaponSkinsRepository.UpdateWeaponSkinAsync(weaponSkin);
            return Ok();
        }

        // Player Weapon Skins

        [ApiKeyAuth]
        [HttpGet("equipment/{playerId}")]
        public async Task<IActionResult> GetPlayerEquippedSkinsAsync(string playerId)
        {
            return Ok(await weaponSkinsRepository.GetPlayerEquipedWeaponSkinsAsync(playerId));
        }

        [Authorize]
        [HttpGet("inventory")]        
        public async Task<IActionResult> GetPlayerWeaponSkinsAsync()
        {
            return Ok(await weaponSkinsRepository.GetPlayerWeaponSkinsAsync(User.Identity.Name));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpGet("inventory/{playerId}")]
        public async Task<IActionResult> GetPlayerWeaponSkinsAsync(string playerId)
        {
            return Ok(await weaponSkinsRepository.GetPlayerWeaponSkinsAsync(playerId));
        }

        [Authorize]
        [HttpPost("equip/{playerWeaponSkinId}")]
        public async Task<IActionResult> PostToggleEquipPlayerWeaponSkinAsync(int playerWeaponSkinId)
        {
            switch (await weaponSkinsRepository.ToggleEquipWeaponSkinAsync(User.Identity.Name, playerWeaponSkinId))
            {
                case 0:
                    return Ok();
                case 1:
                    return NotFound();
                case 2:
                    return BadRequest();
            }
            return BadRequest();
        }
        
        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPost("player")]
        public async Task<IActionResult> PostPlayerWeaponSkinsAsync([FromBody] PlayerWeaponSkin playerWeaponSkin)
        {
            return Ok(await weaponSkinsRepository.AddPlayerWeaponSkinAsync(playerWeaponSkin.WeaponSkinId, playerWeaponSkin.PlayerId));
        } 
    }
}
