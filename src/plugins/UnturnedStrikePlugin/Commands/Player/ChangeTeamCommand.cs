using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;

namespace UnturnedStrike.Plugin.Commands.Player
{
    public class ChangeTeamCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = ((UnturnedPlayer)caller).Player.GetComponent<GamePlayer>();

            if (player == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("ChangeTeamFail"));
                return;
            }

            if (pluginInstance.TeamsService.TeamPlayers[player.TeamType.GetOppositeTeam()].Count >= pluginInstance.TeamsService.TeamPlayers[player.TeamType].Count)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("ChangeTeamFull", player.TeamType.GetOppositeTeam()));
                return;
            }

            pluginInstance.TeamsService.ChangePlayerTeam(player);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "changeteam";

        public string Help => "Changes player's team";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
