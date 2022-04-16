using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class GameServerInfo
    {
        public string Name { get; set; }
        public string Map { get; set; }
        public byte Players { get; set; }
        public byte MaxPlayers { get; set; }
    }
}
