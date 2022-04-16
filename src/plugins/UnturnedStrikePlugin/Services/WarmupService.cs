using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public class WarmupService : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public GameService GameService { get; private set; }

        void Awake()
        {
            GameService = GetComponent<GameService>();
            countdown = pluginInstance.Configuration.Instance.WarmupTime;
        }

        void Start()
        {
            InvokeRepeating("CheckWarmup", 0, 1);
            GameService.TeamsService.OnPlayerTeamJoined += OnPlayerTeamJoined;
            PlayerLife.onPlayerLifeUpdated += OnPlayerLifeUpdated;
            ItemManager.onServerSpawningItemDrop += OnServerSpawningItemDrop;
        }

        void OnDestroy()
        {
            foreach (var player in GameService.TeamsService.Players)
            {
                player.WarmupComponent.Hide();
            }

            GameService.TeamsService.OnPlayerTeamJoined -= OnPlayerTeamJoined;
            PlayerLife.onPlayerLifeUpdated -= OnPlayerLifeUpdated;
            ItemManager.onServerSpawningItemDrop -= OnServerSpawningItemDrop;
        }

        public delegate void WarmupTitleUpdated(string msg);
        public static event WarmupTitleUpdated OnWarmupTitleUpdated;

        private int countdown;

        private void CheckWarmup()
        {
            string countdownString = null;
            if (GameService.TeamsService.IsAnyEmpty())
            {
                // reset countdown
                countdown = pluginInstance.Configuration.Instance.WarmupTime;
            } else 
            {
                countdownString = TimeSpan.FromSeconds(countdown).ToString(@"mm\:ss");
            }

            string msg;
            if (countdownString == null)    
                msg = pluginInstance.Translate("WaitingForPlayersTitle");
            else
                msg = pluginInstance.Translate("WarmupTitle", countdownString);

            OnWarmupTitleUpdated?.Invoke(msg);

            if (--countdown < 0)
            {
                GameService.StartGame();
            }
        }

        private void OnPlayerTeamJoined(GamePlayer player, ETeamType team)
        {
            if (GameService.IsGameStarted)
                return;

            SpawnsHelper.TeleportPlayer(player.NativePlayer, EPlayerSpawnType.Warmup);
            WarmupLoadoutsHelper.GiveLoadout(player);
            player.DisableCosmetics();
        } 

        private void OnPlayerLifeUpdated(Player player)
        {
            if (player.life.isDead)
                return;

            if (GameService.IsGameStarted)
                return;

            var comp = player.GetComponent<GamePlayer>();
            if (comp != null)
                WarmupLoadoutsHelper.GiveLoadout(comp);
        }

        private void OnServerSpawningItemDrop(Item item, ref Vector3 location, ref bool shouldAllow)
        {
            shouldAllow = false;
        }
    }
}
