using SDG.Unturned;
using System.Xml.Serialization;

namespace UnturnedStrike.Plugin.Models
{
    public class JsonWeaponItem
    {
        public JsonWeaponItem(ushort itemId, int amount = 1)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public JsonWeaponItem() { }

        [XmlAttribute]
        public ushort ItemId { get; set; }
        [XmlAttribute]
        public int Amount { get; set; }

        public virtual Item ToItem()
        {
            return new Item(ItemId, true);
        }

        public static JsonWeaponItem FromItem(Item item)
        {
            return new JsonWeaponItem(item.id);
        }
    }
}
