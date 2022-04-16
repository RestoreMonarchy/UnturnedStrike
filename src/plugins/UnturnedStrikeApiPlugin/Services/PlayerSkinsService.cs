using Rocket.Unturned;
using Rocket.Unturned.Player;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnturnedStrike.Plugin;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Services;
using UnturnedStrikeAPI;
using UnturnedStrikeApiPlugin.Providers;
using UnturnedStrikeApiPlugin.Utilities;
using Logger = Rocket.Core.Logging.Logger;


namespace UnturnedStrikeApiPlugin.Services
{
    public class PlayerSkinsService : MonoBehaviour
    {
        private UnturnedStrikeApiPlugin pluginInstance => UnturnedStrikeApiPlugin.Instance;
        private ApiWeaponsProvider WeaponsProvider => pluginInstance.WeaponsProvider;

        internal ApiHttpClient HttpClient { get; set; }
        private GameService CurrentGameService { get; set; }

        void Awake()
        {
            HttpClient = new ApiHttpClient();
        }

        void Start()
        {
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            GameService.OnGameStarted += OnGameStarted;
        }

        void OnDestroy()
        {
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            GameService.OnGameStarted -= OnGameStarted;
        }

        private void OnGameStarted(GameService game)
        {
            CurrentGameService = game;
            CurrentGameService.OnNewRoundCreated += OnNewRoundCreated;
        }

        private void OnNewRoundCreated(int roundId)
        {
            if (CurrentGameService == null)
                return;

            var players = CurrentGameService.TeamsService.Players.ToList();
            ThreadPool.QueueUserWorkItem((_) =>
            {
                foreach (var player in players)
                {
                    UpdatePlayerSkins(player);
                }
            });
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            WeaponsProvider.PlayerWeaponSkins.Remove(player.Id);
        }

        private void UpdatePlayerSkins(GamePlayer player)
        {
            try
            {
                if (!WeaponsProvider.PlayerWeaponSkins.ContainsKey(player.Id))
                {
                    WeaponsProvider.PlayerWeaponSkins
                        .Add(player.Id, HttpClient.GetFromJson<PlayerWeaponSkin[]>($"weaponskins/equipment/{player.Id}"));
                }
                WeaponsProvider.PlayerWeaponSkins[player.Id] 
                    = HttpClient.GetFromJson<PlayerWeaponSkin[]>($"weaponskins/equipment/{player.Id}");
            }
            catch (Exception e)
            {
                Logger.LogException(e, $"An exception occurated while downloading " +
                    $"{player.DisplayName} ({player.Id}) player skins equipment!");
            }
        }
    }
}
