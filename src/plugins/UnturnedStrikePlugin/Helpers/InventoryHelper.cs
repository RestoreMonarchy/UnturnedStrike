using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Helpers
{
    public class InventoryHelper
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public static readonly byte[] EMPTY_BYTE_ARRAY = new byte[0];

        // copied from uEssentials :P
        public static void ClearPlayerInventory(Player player)
        {
            var playerInv = player.inventory;

            // Remove items
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = playerInv.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    playerInv.removeItem(page, 0);
                }
            }

            // Remove clothes

            // Remove unequipped cloths
            System.Action removeUnequipped = () => {
                for (byte i = 0; i < playerInv.getItemCount(2); i++)
                {
                    playerInv.removeItem(2, 0);
                }
            };

            // Unequip & remove from inventory
            player.clothing.askWearBackpack(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearGlasses(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearHat(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearPants(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearMask(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearShirt(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            player.clothing.askWearVest(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped();

            // "Remove "models" of items from player "body""
            player.equipment.sendSlot(0);
            player.equipment.sendSlot(1);
        }

        public static void DropPlayerInventory(Player player)
        {
            try
            {
                for (byte page = 0; page < 6; page++)
                {
                    var count = player.inventory.getItemCount(page);

                    for (byte index = 0; index < count; index++)
                    {
                        player.inventory.sendDropItem(page, player.inventory.getItem(page, 0).x, player.inventory.getItem(page, 0).y);
                    }
                }
            } catch (Exception e)
            {
                Logger.LogError($"Exception occurated when dropping player inventory: {e.Message}");
            }
        }

        public static bool CanHaveMoreGrenades(GamePlayer player)
        {
            return player.NativePlayer.inventory.search(EItemType.THROWABLE).Count < pluginInstance.Configuration.Instance.MaxGrenadesCount;
        }
    }
}
