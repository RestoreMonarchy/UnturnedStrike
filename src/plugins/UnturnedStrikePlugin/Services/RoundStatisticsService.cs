using System.Collections.Generic;
using System.Linq;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public class RoundStatisticsService
    {
        public int RoundID { get; private set; }

        private Dictionary<EMVPType, GamePlayer> Heroes { get; set; }
        private Dictionary<GamePlayer, int> Kills { get; set; } 

        public RoundStatisticsService(int roundId)
        {
            RoundID = roundId;
            Heroes = new Dictionary<EMVPType, GamePlayer>();
            Kills = new Dictionary<GamePlayer, int>();
        }

        public void CountPlayerKill(GamePlayer player)
        {
            if (Kills.ContainsKey(player))
                Kills[player] += 1;
            else
                Kills.Add(player, 1);
        }

        public void ChangeHero(GamePlayer player, EMVPType mvpType)
        {
            Heroes[mvpType] = player;
        }

        public MVP GetMVP(ETeamType team, ERounWinType winType)
        {
            if (winType == ERounWinType.Bomb)
                return new MVP(Heroes[EMVPType.BombPlant], EMVPType.BombPlant);
            else if (winType == ERounWinType.Defuse)
                return new MVP(Heroes[EMVPType.BombDefuse], EMVPType.BombDefuse);
            else if (winType == ERounWinType.Rescue)
                return new MVP(Heroes[EMVPType.HostageRescue], EMVPType.HostageRescue);
            else
                return new MVP(GetTopKiller(team), EMVPType.Eliminations);
        }

        public GamePlayer GetTopKiller(ETeamType team)
        {
            return Kills.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault(x => x.TeamType == team);
        }
    }
}
