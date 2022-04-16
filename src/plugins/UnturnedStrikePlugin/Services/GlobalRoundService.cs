using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public abstract class GlobalRoundService : MonoBehaviour
    {
        protected UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        protected virtual void Initialize() { }
        protected virtual void Begin() { }
        protected virtual void End() { }
        protected virtual void OnTimeElapsed() { }
        protected virtual void OnPlayerRemoved(GamePlayer player) { }
        protected virtual bool CanWinByKills(ETeamType team) => false;
        
        protected GameService GameService { get; private set; }
        protected TeamsService TeamsService => GameService.TeamsService;

        public int RoundID { get; private set; }

        public int TimeLeft { get; protected set; }
        protected int FreezeTimeLeft { get; set; }
        
        protected bool ShouldShowTimeLeft { get; set; }

        public Dictionary<ETeamType, List<GamePlayer>> TeamPlayersAlive { get; private set; }

        public bool IsFreeze { get; private set; }
        public bool Started { get; private set; }

        public bool WasFinished { get; private set; }

        void Awake()
        {
            GameService = GetComponent<GameService>();
            RoundID = GameService.CurrentRoundID;

            TimeLeft = pluginInstance.Configuration.Instance.RoundDuration;
            FreezeTimeLeft = pluginInstance.Configuration.Instance.FreezeTime;

            ShouldShowTimeLeft = true;

            TeamPlayersAlive = new Dictionary<ETeamType, List<GamePlayer>>()
            {
                { ETeamType.Terrorist, new List<GamePlayer>() },
                { ETeamType.CounterTerrorist, new List<GamePlayer>() }
            };

            Initialize();
        }

        void Start()
        {
            ItemManager.askClearAllItems();
            ObjectManager.askClearAllObjects();
            BarricadeManager.askClearAllBarricades();
            LightingManager.time = (uint)(LightingManager.cycle * LevelLighting.transition);

            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;

            StartCoroutine(StartFreezeTime());            
            InvokeRepeating("UpdateTime", 0, 1);
        }

        void OnDestroy()
        {
            if (IsFreeze)
                StopFreezeTime();

            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;

            End();
            GameService.TriggerOnRoundFinished(RoundID);
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            var comp = player.Player.GetComponent<GamePlayer>();
            if (comp != null)
            {
                InventoryHelper.DropPlayerInventory(comp.NativePlayer);
                TeamPlayersAlive[comp.TeamType].Remove(comp);
                GameService.TriggerOnAliveUpdated(comp.TeamType, TeamPlayersAlive[comp.TeamType].Count);
                CheckForKillWin();

                OnPlayerRemoved(comp);
            }
        }

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            var comp = player.Player.GetComponent<GamePlayer>();
            if (comp != null)
            {
                comp.MakeDead(PlayerTool.getPlayer(murderer));
                GameService.StatisticsService.TriggerOnPlayerKilled(comp, comp.LastKiller);

                if (comp.LastKiller != null)
                    comp.LastKiller.GiveMoney(pluginInstance.Configuration.Instance.MoneyRewardKill);

                TeamPlayersAlive[comp.TeamType].Remove(comp);
                GameService.TriggerOnAliveUpdated(comp.TeamType, TeamPlayersAlive[comp.TeamType].Count);
                CheckForKillWin();

                OnPlayerRemoved(comp);
            }
        }

        private IEnumerator StartFreezeTime()
        {
            IsFreeze = true;
            GameService.TriggerOnFreezeStarted();

            foreach (var player in TeamsService.Players)
            {
                player.MakeAlive();
                TeamPlayersAlive[player.TeamType].Add(player);
            }

            yield return new WaitForSeconds(0.75f);            

            SpawnsHelper.TeleportGamePlayers(TeamsService.TeamPlayers[ETeamType.Terrorist], EPlayerSpawnType.Terrorist);
            SpawnsHelper.TeleportGamePlayers(TeamsService.TeamPlayers[ETeamType.CounterTerrorist], EPlayerSpawnType.CounterTerrorist);

            foreach (var player in TeamsService.Players)
            {
                player.FreezePlayer();
                player.TryOpenBuyMenu();

                LoadoutsHelper.GiveLoadout(player);
            }

            GameService.TriggerOnAliveUpdated(ETeamType.Terrorist, TeamPlayersAlive[ETeamType.Terrorist].Count);
            GameService.TriggerOnAliveUpdated(ETeamType.CounterTerrorist, TeamPlayersAlive[ETeamType.CounterTerrorist].Count);

            Begin();
        }

        private void StopFreezeTime()
        {
            IsFreeze = false;
            GameService.TriggerOnFreezeFinished();

            foreach (var player in TeamsService.TeamPlayers.SelectMany(x => x.Value))
            {
                player.UnfreezePlayer();
            }
        }

        protected void StartFinishRound(ETeamType winner, ERounWinType winType)
        {
            if (WasFinished)
                return;

            Started = false;
            if (IsFreeze)
                StopFreezeTime();

            GameService.FinishRound(winner, winType);

            WasFinished = true;
        }

        private void CheckForKillWin()
        {
            foreach (var pair in TeamPlayersAlive)
            {
                if (pair.Value.Count == 0)
                {
                    var winner = TeamPlayersAlive.FirstOrDefault(x => x.Value.Count > 0);

                    if (CanWinByKills(winner.Key))
                    {
                        StartFinishRound(winner.Key, ERounWinType.Eliminate);
                    }                    
                }
            }
        }

        private void UpdateTime()
        {
            if (Started)
            {
                if (TimeLeft <= 0)
                {
                    OnTimeElapsed();
                    return;
                }

                TimeLeft--;

                if (ShouldShowTimeLeft)
                {
                    GameService.TriggerOnTimeUpdated(TimeSpan.FromSeconds(TimeLeft).ToString(@"mm\:ss"));
                }                
            } else if (IsFreeze)
            {
                if (FreezeTimeLeft <= 0)
                {
                    StopFreezeTime();
                    Started = true;
                    GameService.TriggerOnRoundStarted(RoundID);
                    return;
                }

                // Send tick sound to each of player
                if (FreezeTimeLeft < 5)
                    foreach (var player in TeamsService.Players)
                        player.SendSoundEffect(pluginInstance.Configuration.Instance.RoundTickEffectId);

                GameService.TriggerOnTimeUpdated(TimeSpan.FromSeconds(--FreezeTimeLeft).ToString(@"mm\:ss"));
            }
        }        
    }
}
