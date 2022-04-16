using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public class LobbyService : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public Color TerroristColor { get; private set; }
        public Color CounterTerroristColor { get; private set; }
        public Color VIPColor { get; set; }

        void Start()
        {
            TerroristColor = UnturnedChat.GetColorFromName(pluginInstance.Configuration.Instance.TerroristColor, Color.magenta);
            CounterTerroristColor = UnturnedChat.GetColorFromName(pluginInstance.Configuration.Instance.CounterTerroristColor, Color.blue);
            VIPColor = UnturnedChat.GetColorFromName(pluginInstance.Configuration.Instance.ChatVIPColor, Color.yellow);
            U.Events.OnPlayerConnected += OnPlayerConnected;
            ChatManager.onChatted += OnChatted;
        }

        private void OnChatted(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            UnturnedStrikePlayer unturnedStrikePlayer;
            if ((unturnedStrikePlayer = player.player.GetComponent<UnturnedStrikePlayer>()) != null)
            {
                if (unturnedStrikePlayer.IsVIP)
                {
                    chatted = VIPColor;
                    return;
                }   

                GamePlayer gamePlayer = unturnedStrikePlayer as GamePlayer;
                if (gamePlayer != null)
                {
                    if (gamePlayer.TeamType == ETeamType.Terrorist)
                        chatted = TerroristColor;
                    else if (gamePlayer.TeamType == ETeamType.CounterTerrorist)
                        chatted = CounterTerroristColor;
                }
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            SpawnsHelper.TeleportPlayer(player.Player, EPlayerSpawnType.Lobby);
            var lobbyPlayer = player.Player.gameObject.AddComponent<LobbyPlayer>();
            lobbyPlayer.RestoreHealth();
            lobbyPlayer.ShowWaitingUI();
        }

        void OnDestroy()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            ChatManager.onChatted -= OnChatted;
        }
    }
}
