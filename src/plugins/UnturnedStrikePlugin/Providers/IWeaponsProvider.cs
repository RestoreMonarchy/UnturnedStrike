using System.Collections.Generic;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Providers
{
    public interface IWeaponsProvider
    {
        IEnumerable<WeaponModel> Weapons { get; }
        void ReloadWeaponsData();
        void GivePlayerWeapon(GamePlayer player, int weaponId);
        string GetWeaponUnicode(ushort itemId);

        //bool TryBuyWeapon(GamePlayer player, int weaponId);

        //bool TryGetWeaponId(string category, int index, ETeamType team, out int? weaponId);    
    }
}
