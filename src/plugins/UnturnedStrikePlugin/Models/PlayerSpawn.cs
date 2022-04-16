using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UnturnedStrike.Plugin.Models
{
    public class PlayerSpawn
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EPlayerSpawnType Type { get; set; }
        public ConvertableVector3 Position { get; set; }
        public float Rotation { get; set; }
    }
}
