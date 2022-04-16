using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Commands.Other
{
    public class ClearInventoryCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            InventoryHelper.ClearPlayerInventory(player.Player);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "clearinventory";

        public string Help => "Clears inventory";

        public string Syntax => "";

        public List<string> Aliases => new List<string>() { "ci" };

        public List<string> Permissions => new List<string>();
    }
}
