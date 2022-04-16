using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Extensions
{
    public static class ItemExtensions
    {
        /*
            Unturned weapon metadata structure.
            metadata[0] = sight id byte 1
            metadata[1] = sight id byte 2
            metadata[2] = tactical id byte 1
            metadata[3] = tactical id byte 2
            metadata[4] = grip id byte 1
            metadata[5] = grip id byte 2
            metadata[6] = barrel id byte 1
            metadata[7] = barrel id byte 2
            metadata[8] = magazine id byte 1
            metadata[9] = magazine id byte 2
            metadata[10] = ammo
            metadata[11] = firemode
            metadata[12] = ??
            metadata[13] = sight durability
            metadata[14] = tactical durability
            metadata[15] = grip durability
            metadata[16] = barrel durability
            metadata[17] = magazine durability
        */

        public static void ReloadMagazine(this Item item)
        {
            var magAsset = Assets.find(EAssetType.ITEM, item.GetMagazineId()) as ItemMagazineAsset;
            if (magAsset != null)
                item.metadata[10] = magAsset.amount;
        }

        public static ushort GetMagazineId(this Item item)
        {
            return BitConverter.ToUInt16(item.metadata, 8);
        }

        public static bool IsThrowable(this Item item)
        {
            return Assets.find(EAssetType.ITEM, item.id) as ItemThrowableAsset != null;
        }

        public static void AddWeaponItem(this List<JsonWeaponItem> weaponItems, Item item)
        {
            var weaponItem = weaponItems.FirstOrDefault(x => x.ItemId == item.id);
            if (weaponItem != null)
                weaponItem.Amount++;
            else
                weaponItems.Add(new JsonWeaponItem(item.id));
        }        
    }
}
