using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrikeApiPlugin
{
    public class UnturnedStrikeApiConfiguration : IRocketPluginConfiguration
    {
        public string ServerGroup { get; set; }
        public string APIUrl { get; set; }
        public string APIKey { get; set; }        
        public int TimeoutMiliseconds { get; set; }
        public bool UseWeaponsProvider { get; set; }

        public void LoadDefaults()
        {
            ServerGroup = "EU_Main";
            APIUrl = "";
            APIKey = "";
            TimeoutMiliseconds = 8000;
            UseWeaponsProvider = false;
        }
    }
}
