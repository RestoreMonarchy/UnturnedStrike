using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Commands.Player
{
    public class TeamCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;

            var lobbyPlayer = player.Player.GetComponent<LobbyPlayer>();
            if (lobbyPlayer != null)
                lobbyPlayer.TeamsEffectComponent.Open();
            else
                UnturnedChat.Say(caller, pluginInstance.Translate("ChangeTeamInfo"));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "team";

        public string Help => "Displays a teams UI";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
