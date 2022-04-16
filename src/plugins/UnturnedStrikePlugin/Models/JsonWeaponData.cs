using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace UnturnedStrike.Plugin.Models
{
    public class JsonWeaponData : WeaponModel
    {
        public float KillRewardMultiplier { get; set; }
        public JsonWeaponGunItem Weapon { get; set; }
        public JsonWeaponItem[] Items { get; set; }
    }
}
