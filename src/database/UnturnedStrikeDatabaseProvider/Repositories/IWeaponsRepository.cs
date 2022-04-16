using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IWeaponsRepository
    {
        Task<IEnumerable<Weapon>> GetWeaponsAsync();
        Task<Weapon> GetWeaponAsync(int weaponId);
        Task<Weapon> AddWeaponAsync(Weapon weapon);
        Task UpdateWeaponAsync(Weapon weapon);
        Task<IEnumerable<Weapon>> GetEnabledWeaponsAsync();
    }
}
