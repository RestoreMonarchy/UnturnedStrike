using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class MakeRegionCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 2 || !float.TryParse(command[1], out float height))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("MakeRegionInvalid"));
                return;
            }

            RegionsHelper.MakeRegion(command[0][0], height, out var region);
            UnturnedChat.Say(caller, pluginInstance.Translate("MakeRegionSuccess", region.Character, 
                region.Nodes[0].ToString(), region.Nodes[1].ToString()));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "makeregion";

        public string Help => "Makes a region from nodes";

        public string Syntax => "<name> <height>";

        public List<string> Aliases => new List<string>() { };

        public List<string> Permissions => new List<string>();
    }
}
