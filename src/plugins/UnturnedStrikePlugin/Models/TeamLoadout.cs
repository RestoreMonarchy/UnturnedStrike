using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class TeamLoadout
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ETeamType Team { get; set; }
        public int WeaponID { get; set; }
        public TeamLoadoutClothing[] Clothes { get; set; }
        public TeamLoadoutSkill[] Skills { get; set; }
    }
}
