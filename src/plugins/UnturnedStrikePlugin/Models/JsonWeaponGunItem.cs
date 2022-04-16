using SDG.Unturned;
using System;

namespace UnturnedStrike.Plugin.Models
{
    public class JsonWeaponGunItem : JsonWeaponItem
    {
        public ushort SightId { get; set; }        
        public ushort TacticalId { get; set; }
        public ushort GripId { get; set; }
        public ushort BarrelId { get; set; }
        public ushort MagazineId { get; set; }
        public byte Ammo { get; set; }

        public override Item ToItem()
        {
            var asset = Assets.find(EAssetType.ITEM, ItemId) as ItemGunAsset;
            if (asset == null || MagazineId == 0)
                return new Item(ItemId, true);
            return new Item(ItemId, 1, 100, asset.getState(SightId, TacticalId, GripId, BarrelId, MagazineId, Ammo));
        }

        public static new JsonWeaponGunItem FromItem(Item item)
        {
            return new JsonWeaponGunItem()
            {
                ItemId = item.id,
                Amount = 1,
                SightId = BitConverter.ToUInt16(item.metadata, 0),
                TacticalId = BitConverter.ToUInt16(item.metadata, 2),
                GripId = BitConverter.ToUInt16(item.metadata, 4),
                BarrelId = BitConverter.ToUInt16(item.metadata, 6),                
                MagazineId = BitConverter.ToUInt16(item.metadata, 8),
                Ammo = item.metadata[10]
            };            
        }
    }
}
