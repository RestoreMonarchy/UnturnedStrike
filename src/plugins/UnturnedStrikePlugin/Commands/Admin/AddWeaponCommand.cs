using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Providers;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class AddWeaponCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var provider = pluginInstance.WeaponsProvider as JsonWeaponsProvider;
            if (provider == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("AddWeaponCommandDisabled"));
                return;
            }


            if (command.Length < 1 || !int.TryParse(command[0], out int price))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("AddWeaponInvalid"));
                return;
            }

            var player = (UnturnedPlayer)caller;

            var weaponData = new JsonWeaponData()
            {
                KillRewardMultiplier = 1.0f,
                Team = EWeaponTeam.Both,
                Price = price
            };

            var items = new List<JsonWeaponItem>();

            for (byte page = 0; page <= PlayerInventory.PANTS; page++)
            {
                for (byte i = 0; i < player.Inventory.getItemCount(page); i++)
                {
                    var item = player.Inventory.getItem(page, i);
                    if (weaponData.Weapon == null && item.item.metadata.Length > 16)
                    {
                        weaponData.Weapon = JsonWeaponGunItem.FromItem(item.item);
                        weaponData.Name = (Assets.find(EAssetType.ITEM, item.item.id) as ItemGunAsset).itemName;
                    }
                    else
                        items.AddWeaponItem(item.item);
                }
            }

            if (weaponData.Name == null)
            {
                var weapon = items.FirstOrDefault();
                if (weapon != null)
                    weaponData.Name = (Assets.find(EAssetType.ITEM, weapon.ItemId) as ItemAsset).itemName;
            }

            weaponData.Items = items.ToArray();
            
            UnturnedChat.Say(caller, pluginInstance.Translate("AddWeaponSuccess", weaponData.Id, weaponData.Name, weaponData.Items.Length));

            
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "addweapon";

        public string Help => "Adds a new weapon to weaponsdata";

        public string Syntax => "<price>";

        public List<string> Aliases => new List<string>() { "aw" };

        public List<string> Permissions => new List<string>();
    }
}
