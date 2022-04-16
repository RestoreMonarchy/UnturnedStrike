using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class TeamLoadoutClothing
    {
        public TeamLoadoutClothing() { }

        public TeamLoadoutClothing(ushort itemId, float armor)
        {
            ItemId = itemId;
            Armor = armor;
        }

        public ushort ItemId { get; set; }
        public float Armor { get; set; }
    }
}
