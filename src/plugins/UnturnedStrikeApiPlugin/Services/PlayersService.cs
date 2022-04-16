using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrikeAPI;
using UnturnedStrikeApiPlugin.Utilities;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrikeApiPlugin.Services
{
    public class PlayersService : MonoBehaviour
    {
        private UnturnedStrikeApiPlugin pluginInstance => UnturnedStrikeApiPlugin.Instance;
        private ApiHttpClient HttpClient { get; set; }

        public Dictionary<CSteamID, Player> Players { get; set; }

        void Awake()
        {
            Players = new Dictionary<CSteamID, Player>();
            HttpClient = new ApiHttpClient();
        }

        void Start()
        {
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        void OnDestroy()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    UpdateGetPlayer(player);
                }
                catch (Exception e)
                {
                    Logger.LogException(e, "An exception occurated while processing player connected!");
                }
            });
        }

        public void UpdateGetPlayer(UnturnedPlayer player)
        {
            Players.Add(player.CSteamID, HttpClient.GetFromJson<Player>($"players/{player.Id}"));

            var apiPlayer = new Player()
            {
                Id = player.Id,
                IP = SDG.Unturned.Parser.getIPFromUInt32(player.Player.channel.owner.getIPv4AddressOrZero())
            };
            if (Players[player.CSteamID] == null)
            {
                Players[player.CSteamID] = HttpClient.SendAsJson<Player>($"players", apiPlayer);
            }
            else
            {
                Players[player.CSteamID] = HttpClient.SendAsJson<Player>($"players", apiPlayer, "PUT");
            }

            if (Players[player.CSteamID].IsVIP)
            {
                TaskDispatcher.QueueOnMainThread(() =>
                    VIPHelper.VIPPlayers.Add(player.CSteamID, DateTime.UtcNow.AddDays(365)));
            }
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (VIPHelper.VIPPlayers.ContainsKey(player.CSteamID))
                VIPHelper.VIPPlayers.Remove(player.CSteamID);

            Players.Remove(player.CSteamID);
        }
    }
}
