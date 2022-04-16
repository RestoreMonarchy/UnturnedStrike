using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class TeamLoadoutSkill
    {
        public TeamLoadoutSkill() { }
        public TeamLoadoutSkill(string name, byte level)
        {
            Name = name;
            Level = level;
        }

        public string Name { get; set; }
        public byte Level { get; set; }
    }
}
