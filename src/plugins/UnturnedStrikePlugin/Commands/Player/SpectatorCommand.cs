using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Commands.Player
{
    public class SpectatorCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;

            SpectatorPlayer spectatorPlayer = player.Player.GetComponent<SpectatorPlayer>();
            LobbyPlayer lobbyPlayer;
            if (spectatorPlayer != null)
            {
                spectatorPlayer.SelfDestroy();
                player.Player.gameObject.AddComponent<LobbyPlayer>();
                UnturnedChat.Say(caller, pluginInstance.Translate("SpectatorModeOff"));
                return;
            }

            lobbyPlayer = player.Player.GetComponent<LobbyPlayer>();
            if (lobbyPlayer == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("SpectatorFail"));
                return;
            }

            lobbyPlayer.SelfDestroy();
            spectatorPlayer = player.Player.gameObject.AddComponent<SpectatorPlayer>();
            pluginInstance.SpectatorService.AddSpectator(spectatorPlayer);

            UnturnedChat.Say(caller, pluginInstance.Translate("SpectatorModeOn"));
        }

        public string Name => "spectator";
        public string Help => "Spectator commands";
        public string Syntax => "";
        public List<string> Aliases => new();
        public List<string> Permissions => new();
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
    }
}
