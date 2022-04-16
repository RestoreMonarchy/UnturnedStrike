using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class GameServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public DateTime CreateDate { get; set; }

        public GameServerInfo Info { get; set; }

        [JsonIgnore]
        public bool IsOnline => Info != null;
    }
}
