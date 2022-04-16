using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace UnturnedStrike.Plugin.Models
{
    public class TeamClothes
    {
        [XmlAttribute]
        public ETeamType Team { get; set; }
        [XmlArrayItem("ClothingID")]
        public ushort[] Clothes { get; set; }
    }
}
