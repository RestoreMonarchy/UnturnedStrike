using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class GameResults
    {
        public int GameId { get; set; }
        public EGameType GameType { get; set; }
        public ETeamType Winner { get; set; }
        public EGameWinType GameWinType { get; set; }
        public byte TerroristScore { get; set; }
        public byte CounterTerroristScore { get; set; }
        public IEnumerable<PlayerStat> Stats { get; set; }
    }
}
