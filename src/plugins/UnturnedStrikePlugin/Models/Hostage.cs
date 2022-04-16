using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Models
{
    public class Hostage
    {
        public int Id { get; set; }
        public ConvertableVector3 Position { get; set; }
        public ConvertableVector3 Angles { get; set; }
    }
}
