using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin.Services
{
    public class GameStatisticsService : MonoBehaviour
    {
        private GameService GameService { get; set; }

        public Dictionary<CSteamID, PlayerStat> Stats { get; private set; }

        public RoundStatisticsService RoundStatistics { get; private set; }

        #region Events
        public delegate void PlayerKilled(GamePlayer victim, GamePlayer killer);
        public delegate void BombPlanted(GamePlayer planter);
        public delegate void BombDefused(GamePlayer defuser);
        public delegate void HostageRescued(GamePlayer rescuer);

        public event PlayerKilled OnPlayerKilled;
        public event BombPlanted OnBombPlanted;
        public event BombDefused OnBombDefused;
        public event HostageRescued OnHostageRescued;

        public void TriggerOnPlayerKilled(GamePlayer victim, GamePlayer killer)
        {
            OnPlayerKilled?.Invoke(victim, killer);
        }

        public void TriggerOnBombPlanted(GamePlayer planter)
        {
            OnBombPlanted?.Invoke(planter);
        }

        public void TriggerOnBombDefused(GamePlayer defuser)
        {
            OnBombDefused?.Invoke(defuser);
        }

        public void TriggerOnHostageRescued(GamePlayer rescuer)
        {
            OnHostageRescued?.Invoke(rescuer);
        }

        #endregion

        void Awake()
        {
            Stats = new Dictionary<CSteamID, PlayerStat>();
            GameService = GetComponent<GameService>();
        }

        void Start()
        {
            GameService.OnNewRoundCreated += OnNewRoundCreated;
            GameService.OnRoundWon += OnRoundWon;
            GameService.TeamsService.OnPlayerTeamJoined += AddPlayer;
            GameService.TeamsService.OnPlayerTeamLeft += RemovePlayer;
            OnPlayerKilled += ScorePlayerKill;
            OnBombPlanted += ScoreBombPlanted;
            OnBombDefused += ScoreBombDefused;
            OnHostageRescued += ScoreHostageRescued;
        }

        private void RemovePlayer(GamePlayer player, ETeamType team)
        {
            if (Stats.ContainsKey(player.CSteamID))
                Stats[player.CSteamID].IsInTeam = false;
        }

        public IEnumerable<PlayerStat> GetPlayerStats()
        {
            return Stats.Select(x => x.Value);
        }

        private void OnRoundWon(ETeamType winner, ERounWinType winType, MVP mvp)
        {
            if (mvp.Player != null)
                Stats[mvp.Player.CSteamID].MVPs++;
        }

        private void OnNewRoundCreated(int roundId)
        {
            RoundStatistics = new RoundStatisticsService(roundId);
        }

        private void AddPlayer(GamePlayer player, ETeamType team)
        {
            if (!Stats.ContainsKey(player.CSteamID))
            {
                Stats.Add(player.CSteamID, new PlayerStat(player.CSteamID, player.DisplayName, team));
                new Task(GetPlayerIcon).Start();
                void GetPlayerIcon()
                {
                    var iconUrl = UnturnedPlayer.FromPlayer(player.NativePlayer).SteamProfile.AvatarIcon.ToString();
                    if (Stats.ContainsKey(player.CSteamID))
                    {
                        Stats[player.CSteamID].IconUrl = iconUrl;
                    }
                }
                Logger.Log($"{player.DisplayName} added to stats!");
            }

            Stats[player.CSteamID].Team = player.TeamType;
            Stats[player.CSteamID].IsInTeam = true;
        }

        private void ScorePlayerKill(GamePlayer victim, GamePlayer killer)
        {
            Stats[victim.CSteamID].Deaths++;
            if (killer != null)
            {
                Stats[killer.CSteamID].Kills++;
                RoundStatistics.CountPlayerKill(killer);
            }
        }

        private void ScoreBombPlanted(GamePlayer planter)
        {
            Stats[planter.CSteamID].BombPlants++;
            RoundStatistics.ChangeHero(planter, EMVPType.BombPlant);
        }

        private void ScoreBombDefused(GamePlayer defuser)
        {
            Stats[defuser.CSteamID].BombDefuses++;
            RoundStatistics.ChangeHero(defuser, EMVPType.BombDefuse);
        }

        private void ScoreHostageRescued(GamePlayer rescuer)
        {
            Stats[rescuer.CSteamID].HostageRescues++;
            RoundStatistics.ChangeHero(rescuer, EMVPType.HostageRescue);
        }

        public IEnumerable<PlayerStat> GetPlayingPlayersStats()
        {
            return Stats.Select(x => x.Value).Where(x => x.IsInTeam);
        }
            
        void OnDestroy()
        {
            GameService.OnNewRoundCreated -= OnNewRoundCreated;
            GameService.OnRoundWon -= OnRoundWon;
            GameService.TeamsService.OnPlayerTeamJoined -= AddPlayer;
            GameService.TeamsService.OnPlayerTeamLeft -= RemovePlayer;
            OnPlayerKilled -= ScorePlayerKill;
            OnBombPlanted -= ScoreBombPlanted;
            OnBombDefused -= ScoreBombDefused;
        }
    }
}
