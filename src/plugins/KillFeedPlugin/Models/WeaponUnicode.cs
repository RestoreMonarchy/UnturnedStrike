using System.Xml.Serialization;

namespace KillFeedPlugin.Models
{
    public class WeaponUnicode
    {
        public WeaponUnicode() { }

        public WeaponUnicode(ushort weaponId, string unicode)
        {
            WeaponId = weaponId;
            Unicode = unicode;
        }

        [XmlAttribute]
        public ushort WeaponId { get; set; }
        [XmlAttribute]
        public string Unicode { get; set; }
    }
}
