using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class RestartGameCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (pluginInstance.GameService == null || !pluginInstance.GameService.IsGameStarted)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("GameRestartFail"));
                return;
            }

            pluginInstance.GameService.StartFinishGame(EGameWinType.Restart, ETeamType.Terrorist);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "restartgame";

        public string Help => "Restarts the game";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
