using System.Collections.Generic;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IWeaponSkinsRepository
    {
        Task<IEnumerable<WeaponSkin>> GetWeaponSkinsAsync();
        Task<WeaponSkin> GetWeaponSkinAsync(int weaponSkinId);
        Task<WeaponSkin> AddWeaponSkinAsync(WeaponSkin weapon);
        Task UpdateWeaponSkinAsync(WeaponSkin weapon);
                
        Task<PlayerWeaponSkin> GetPlayerWeaponSkinAsync(int weaponSkinId);
        Task<IEnumerable<PlayerWeaponSkin>> GetPlayerEquipedWeaponSkinsAsync(string playerId);
        Task<IEnumerable<PlayerWeaponSkin>> GetPlayerWeaponSkinsAsync(string playerId);
        Task<PlayerWeaponSkin> AddPlayerWeaponSkinAsync(int weaponSkinId, string playerId);
        Task<int> ToggleEquipWeaponSkinAsync(string playerId, int playerWeaponSkinId);
    }
}
