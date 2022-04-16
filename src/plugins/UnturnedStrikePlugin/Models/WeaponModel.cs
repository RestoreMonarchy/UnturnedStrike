using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class WeaponModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EWeaponTeam Team { get; set; }
    }
}
