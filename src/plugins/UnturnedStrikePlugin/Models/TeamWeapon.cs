using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnturnedStrike.Plugin.Models
{
    public class TeamWeapon
    {
        [XmlAttribute]
        public ETeamType Team { get; set; }
        [XmlAttribute]
        public int WeaponID { get; set; }
    }
}
