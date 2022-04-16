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
    public class WeaponsController : ControllerBase
    {
        private readonly IWeaponsRepository weaponsRepository;

        public WeaponsController(IWeaponsRepository weaponsRepository)
        {
            this.weaponsRepository = weaponsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if (User.IsInRole(RoleConstants.AdminRoleId))
            {
                return Ok(await weaponsRepository.GetWeaponsAsync());
            } else
            {
                return Ok(await weaponsRepository.GetEnabledWeaponsAsync());
            }
            
        }

        [HttpGet("{weaponId}")]
        public async Task<IActionResult> GetWeaponAsync(int weaponId)
        {
            return Ok(await weaponsRepository.GetWeaponAsync(weaponId));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Weapon weapon)
        {
            return Ok(await weaponsRepository.AddWeaponAsync(weapon));
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Weapon weapon)
        {
            await weaponsRepository.UpdateWeaponAsync(weapon);
            return Ok();
        }
    }
}
