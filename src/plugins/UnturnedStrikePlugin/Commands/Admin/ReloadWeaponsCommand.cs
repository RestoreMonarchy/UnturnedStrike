using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class ReloadWeaponsCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            pluginInstance.WeaponsProvider.ReloadWeaponsData();
            UnturnedChat.Say(caller, pluginInstance.Translate("ReloadWeaponsSuccess"));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "reloadweapons";

        public string Help => "Reloads weaponsdata";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
