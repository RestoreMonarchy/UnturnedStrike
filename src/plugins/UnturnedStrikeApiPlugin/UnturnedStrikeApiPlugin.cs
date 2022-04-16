using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using UnturnedStrikeApiPlugin.Providers;
using UnturnedStrikeApiPlugin.Services;
using UnturnedStrikeApiPlugin.Utilities;

namespace UnturnedStrikeApiPlugin
{
    public class UnturnedStrikeApiPlugin : RocketPlugin<UnturnedStrikeApiConfiguration>
    {
        public static UnturnedStrikeApiPlugin Instance { get; private set; }
        internal ApiHttpClient HttpClient { get; set; }
        internal ApiWeaponsProvider WeaponsProvider { get; set; }


        public PlayersService PlayersService { get; private set; }
        public GamesService GamesService { get; private set; }
        public PlayerSkinsService PlayerSkinsService { get; private set; }


        protected override void Load()
        {            
            Instance = this;
            HttpClient = new ApiHttpClient();

            if (Configuration.Instance.UseWeaponsProvider)
            {
                WeaponsProvider = new ApiWeaponsProvider();
                PlayerSkinsService = gameObject.AddComponent<PlayerSkinsService>();
            }

            PlayersService = gameObject.AddComponent<PlayersService>();
            GamesService = gameObject.AddComponent<GamesService>();

            R.Plugins.OnPluginsLoaded += OnUnturnedStrikePluginLoaded;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        private void OnUnturnedStrikePluginLoaded()
        {
            if (Configuration.Instance.UseWeaponsProvider)
            {
                UnturnedStrike.Plugin.UnturnedStrikePlugin.Instance.WeaponsProvider = WeaponsProvider;                
            }
        }

        protected override void Unload()
        {
            R.Plugins.OnPluginsLoaded -= OnUnturnedStrikePluginLoaded;
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }
    }
}