using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class UserInfo
    {
        public string SteamID { get; set; }
        public string Role { get; set; }
        public bool IsAuthenticated { get; set; }
        public Player Player { get; set; }

        [JsonIgnore]
        public bool HasPlayer => Player != null;
    }
}
