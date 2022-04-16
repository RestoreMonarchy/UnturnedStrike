using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;

namespace UnturnedStrike.Plugin.Providers
{
    public class JsonWeaponsProvider : IWeaponsProvider
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private int NewID 
        {
            get
            {
                int lastId = weaponsData.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
                return ++lastId;
            }
        }

        private DataStorage<List<JsonWeaponData>> WeaponsDataStorage { get; set; }

        private List<JsonWeaponData> weaponsData;

        public IEnumerable<WeaponModel> Weapons => weaponsData;

        public JsonWeaponsProvider()
        {
            WeaponsDataStorage = new DataStorage<List<JsonWeaponData>>(pluginInstance.Directory, "WeaponsData.json");
        }

        public void ReloadWeaponsData()
        {
            weaponsData = WeaponsDataStorage.Read();
            if (weaponsData == null)
                weaponsData = new List<JsonWeaponData>();

            Logger.Log($"Weapons loaded: {weaponsData.Count}", ConsoleColor.Yellow);
        }

        public void GivePlayerWeapon(GamePlayer player, int weaponId)
        {
            var weaponData = weaponsData.FirstOrDefault(x => x.Id == weaponId);
            
            if (weaponData == null)
                return;

            if (weaponData.Weapon != null)
            {
                player.NativePlayer.inventory.forceAddItem(weaponData.Weapon.ToItem(), true);
            }

            foreach (var weaponItem in weaponData.Items)
            {
                for (int i = 0; i < weaponItem.Amount; i++)
                    player.NativePlayer.inventory.forceAddItem(weaponItem.ToItem(), false);
            }
        }


        // Extra methods
        public void AddWeapon(JsonWeaponData weaponData)
        {
            weaponData.Id = NewID;
            weaponsData.Add(weaponData);
            WeaponsDataStorage.Save(weaponsData);
        }

        public bool RemoveWeapon(int weaponId, out JsonWeaponData weaponData)
        {
            weaponData = weaponsData.FirstOrDefault(x => x.Id == weaponId);
            if (weaponData != null)
            {
                weaponsData.Remove(weaponData);
                WeaponsDataStorage.Save(weaponsData);
                return true;
            }
            return false;
        }

        public string GetWeaponUnicode(ushort itemId)
        {
            return null;
        }
    }
}
