using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Models
{
    public class MVP
    {
        public MVP(GamePlayer player, EMVPType type)
        {
            Player = player;
            Type = type;
        }

        public GamePlayer Player { get; set; }
        public EMVPType Type { get; set; }
    }
}
