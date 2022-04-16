using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Providers;
using UnturnedStrikeAPI;

namespace UnturnedStrikeApiPlugin.Providers
{
    public class ApiWeaponsProvider : IWeaponsProvider
    {
        private UnturnedStrikeApiPlugin pluginInstance => UnturnedStrikeApiPlugin.Instance;

        private IEnumerable<Weapon> weapons = new Weapon[0];
        private IEnumerable<WeaponSkin> weaponSkins = new WeaponSkin[0];

        public IEnumerable<WeaponModel> Weapons { get; private set; }
        internal Dictionary<string, IEnumerable<PlayerWeaponSkin>> PlayerWeaponSkins { get; }

        public ApiWeaponsProvider()
        {
            PlayerWeaponSkins = new Dictionary<string, IEnumerable<PlayerWeaponSkin>>();
        }

        public void ReloadWeaponsData()
        {
            weapons = pluginInstance.HttpClient.GetFromJson<List<Weapon>>("weapons");
            
            weaponSkins = pluginInstance.HttpClient.GetFromJson<List<WeaponSkin>>("weaponskins");
            if (weapons != null)
            {
                List<WeaponModel> weaponModels = new List<WeaponModel>();
                foreach (var weapon in weapons)
                {
                    weaponModels.Add(new WeaponModel()
                    {
                        Id = weapon.Id,
                        Name = weapon.Name,
                        Category = weapon.Category,
                        Price = weapon.Price,
                        Team = (EWeaponTeam)Enum.Parse(typeof(EWeaponTeam), weapon.Team)
                    });
                }
                Weapons = weaponModels;
            }
        }

        public string GetWeaponUnicode(ushort itemId)
        {
            Weapon weapon;
            if ((weapon = weapons.FirstOrDefault(x => x.ItemId == itemId)) != null)
                return weapon.IconUnicode;

            WeaponSkin weaponSkin;
            if ((weaponSkin = weaponSkins.FirstOrDefault(x => x.ItemId == itemId)) != null)
                return weaponSkin.Weapon.IconUnicode;

            return null;
        }

        public void GivePlayerWeapon(GamePlayer player, int weaponId)
        {
            var weapon = weapons.FirstOrDefault(x => x.Id == weaponId);
            
            if (weapon == null)
                return;

            ushort itemId = (ushort)weapon.ItemId;

            if (PlayerWeaponSkins.ContainsKey(player.Id))
            {
                var skin = PlayerWeaponSkins[player.Id]?.FirstOrDefault(x => x.WeaponSkin.WeaponId == weaponId) ?? null;
                if (skin != null)
                {
                    itemId = (ushort)skin.WeaponSkin.ItemId;
                }
            }            

            // Giving weapon
            player.NativePlayer.inventory.forceAddItem(new Item(itemId, true), true);

            if (weapon.MagazineId.HasValue)
            {
                for (int i = 0; i < weapon.MagazineAmount; i++)
                {
                    player.NativePlayer.inventory.forceAddItem(new Item((ushort)weapon.MagazineId, true), false);
                }                
            }
        }
    }
}
