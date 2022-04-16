using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class SetNodeCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            RegionsHelper.Nodes.Add(new ConvertableVector3(player.Position));
            UnturnedChat.Say(caller, pluginInstance.Translate("SetNodeSuccess", player.Position));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "setnode";

        public string Help => "Sets node in your position";

        public string Syntax => "";

        public List<string> Aliases => new List<string>() { };

        public List<string> Permissions => new List<string>();
    }
}
