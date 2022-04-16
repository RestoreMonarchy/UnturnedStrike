using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Commands.Player
{
    public class TeamsCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var teams = pluginInstance.TeamsService;
            
            UnturnedChat.Say(caller, pluginInstance.Translate("TeamsDisplay", teams.TeamPlayers[ETeamType.Terrorist].Count, 
                teams.IsTeamFull(ETeamType.Terrorist) ? "" : "*", 
                teams.TeamPlayers[ETeamType.CounterTerrorist].Count, 
                teams.IsTeamFull(ETeamType.CounterTerrorist) ? "" : "*"));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "teams";

        public string Help => "Shows available teams";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
