using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;

namespace UnturnedStrike.Plugin.Helpers
{
    public class WarmupLoadoutsHelper
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private static IEnumerable<WeaponModel> Guns => pluginInstance.WeaponsProvider.Weapons.Where(
            x => x.Category == "Rifles" || x.Category == "Heavy" || x.Category == "SMGs");

        private static System.Random random = new System.Random();

        public static void GiveLoadout(GamePlayer player)
        {
            player.ClearInventory();

            // give random gun
            var guns = Guns.ToList();
            pluginInstance.WeaponsProvider.GivePlayerWeapon(player, guns[random.Next(0, guns.Count)].Id);

            LoadoutsHelper.GiveLoadout(player);
        }
    }
}
